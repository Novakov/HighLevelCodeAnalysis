using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using CodeModel.Builder;
using CodeModel.MonadicParser;
using Mono.Reflection;

namespace CodeModel.Extensions.Nancy
{
    public class DetectNancyRoutes : INodeMutator<NancyModuleNode>
    {
        public void Mutate(NancyModuleNode node, IMutateContext context)
        {
            var ctor = node.Type.GetConstructors().Single();

            var instructions = ctor.GetInstructions();
            
            var routes = FindRoutes(instructions);

            foreach (var route in routes)
            {
                context.AddNode(new NancyRouteNode(route.Method, route.Path, route.RouteBuilder.Name.Substring(4)));
            }            
        }

        private static IEnumerable<Route> FindRoutes(IEnumerable<Instruction> instructions)
        {
            var parser = RoutePatternParser();

            var remainingInstructions = new List<Instruction>(instructions);            

            while (remainingInstructions.Any())
            {
                var result = parser(remainingInstructions);

                if (result == null)
                {
                    remainingInstructions.RemoveAt(0);
                }
                else
                {
                    remainingInstructions = new List<Instruction>(result.Rest);
                    yield return result.Value;
                }
            }            
        }

        private static Parser<IEnumerable<Instruction>, Route> RoutePatternParser()
        {
            var useAnonymousDelegate = from load in Parsers.AnyOf(IlParser.OpCode(OpCodes.Ldsfld), IlParser.LoadLocal())
                from branchToEnd in Parsers.AnyOf(IlParser.OpCode(OpCodes.Brtrue), IlParser.OpCode(OpCodes.Brtrue_S))
                from delegateTarget in IlParser.Any()
                from loadFtn in IlParser.OpCode(OpCodes.Ldftn)
                from createDelegate in IlParser.OpCode(OpCodes.Newobj)
                from store in Parsers.AnyOf(IlParser.OpCode(OpCodes.Stsfld), IlParser.StoreLocal())
                from _ in IlParser.OpCode(OpCodes.Br_S).Optional()
                from load2 in Parsers.AnyOf(IlParser.OpCode(OpCodes.Ldsfld), IlParser.LoadLocal()) //TODO: add condition                                       
                select new {Method = (MethodInfo) loadFtn.Operand};

            var lambdaNoClosure = from loadThis in IlParser.OpCode(OpCodes.Ldarg_0)
                from callGetRouteBuilder in IlParser.OpCode(OpCodes.Call) //TODO: add condition
                from ldstrPath in IlParser.OpCode(OpCodes.Ldstr)
                from actionDelegateStore in useAnonymousDelegate
                from callSetItem in IlParser.OpCode(OpCodes.Callvirt) //TODO: add codition
                select new Route
                {
                    RouteBuilder = (MethodInfo) callGetRouteBuilder.Operand,
                    Path = (string) ldstrPath.Operand,
                    Method = actionDelegateStore.Method
                };

            var lambdaWithClosure = from getRouteBuilder in IlParser.OpCode(OpCodes.Call)
                from ldstrPath in IlParser.OpCode(OpCodes.Ldstr)
                from loadClosure in IlParser.LoadLocal()
                from loadftn in IlParser.OpCode(OpCodes.Ldftn)
                from createDelegate in IlParser.OpCode(OpCodes.Newobj)
                from callSetItem in IlParser.OpCode(OpCodes.Callvirt)
                select new Route()
                {
                    RouteBuilder = (MethodInfo) getRouteBuilder.Operand,
                    Path = (string) ldstrPath.Operand,
                    Method = (MethodInfo) loadftn.Operand
                };

            var parser = Parsers.AnyOf(lambdaNoClosure, lambdaWithClosure);
            return parser;
        }

        private class Route
        {
            public MethodInfo RouteBuilder { get; set; }
            public MethodInfo Method { get; set; }
            public string Path { get; set; }
        }
    }
}