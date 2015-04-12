using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using CodeModel.Builder;
using CodeModel.Extensions.Nancy;
using CodeModel.Extensions.Nancy.Mutators;
using CodeModel.MonadicParser;
using CodeModel.Primitives;
using Mono.Reflection;
using NUnit.Framework;
using Tests.Constraints;
using TestTarget.NancyLib;

namespace Tests.Extensions.Nancy
{
    public class MutatorTests : IHaveBuilder
    {
        public CodeModelBuilder Builder { get; set; }

        [SetUp]
        public void SetUp()
        {
            this.Builder = new CodeModelBuilder();
        }

        [Test]
        public void ShouldDetectNancyModules()
        {
            // arrange
            this.Builder.Model.AddNode(new TypeNode(typeof(MyModule)));

            // act
            this.Builder.RunMutator<DetectNancyModules>();

            // assert
            Assert.That(this.Builder.Model, Graph.Has
                .Nodes<NancyModuleNode>(exactly: 1, matches: x => x.Type == typeof(MyModule))
                );
        }

        [Test]
        public void X()
        {
            var routes = GetRoutesInModule(typeof(MyModule));

            Console.WriteLine("Found routes:");
            foreach (var route in routes)
            {
                Console.WriteLine("{0}['{1}'] = {2}", route.RouteBuilder.Name.Substring(4), route.Path, route.Method);
            }

            Assert.That(routes, Has.Count.EqualTo(4));
        }

        private static List<MutatorTests.Route> GetRoutesInModule(Type moduleType)
        {
            var ctor = moduleType.GetConstructors().Single();

            var instructions = ctor.GetInstructions();

            Console.WriteLine("Instructions = {0}", instructions.Count);

            foreach (var instruction in instructions)
            {
                Console.WriteLine(instruction);
            }

            var useAnonymousDelegate = from load in Parsers.AnyOf(IlParser.OpCode(OpCodes.Ldsfld), IlParser.LoadLocal())
                                       from branchToEnd in Parsers.AnyOf(IlParser.OpCode(OpCodes.Brtrue), IlParser.OpCode(OpCodes.Brtrue_S))
                                       from delegateTarget in IlParser.Any()
                                       from loadFtn in IlParser.OpCode(OpCodes.Ldftn)
                                       from createDelegate in IlParser.OpCode(OpCodes.Newobj)
                                       from store in Parsers.AnyOf(IlParser.OpCode(OpCodes.Stsfld), IlParser.StoreLocal())
                                       from _ in IlParser.OpCode(OpCodes.Br_S).Optional()
                                       from load2 in Parsers.AnyOf(IlParser.OpCode(OpCodes.Ldsfld), IlParser.LoadLocal()) //TODO: add condition                                       
                                       select new { Method = (MethodInfo)loadFtn.Operand };

            var lambdaNoClosure = from loadThis in IlParser.OpCode(OpCodes.Ldarg_0)
                                  from callGetRouteBuilder in IlParser.OpCode(OpCodes.Call) //TODO: add condition
                                  from ldstrPath in IlParser.OpCode(OpCodes.Ldstr)
                                  from actionDelegateStore in useAnonymousDelegate
                                  from callSetItem in IlParser.OpCode(OpCodes.Callvirt) //TODO: add codition
                                  select new Route
                                  {
                                      RouteBuilder = (MethodInfo)callGetRouteBuilder.Operand,
                                      Path = (string)ldstrPath.Operand,
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
                                        RouteBuilder = (MethodInfo)getRouteBuilder.Operand,
                                        Path = (string)ldstrPath.Operand,
                                        Method = (MethodInfo)loadftn.Operand
                                    };

            var parser = Parsers.AnyOf(lambdaNoClosure, lambdaWithClosure);

            var remainingInstructions = new List<Instruction>(instructions);

            var routes = new List<Route>();

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
                    routes.Add(result.Value);
                }
            }
            return routes;
        }

        public class Route
        {
            public MethodInfo RouteBuilder { get; set; }
            public MethodInfo Method { get; set; }
            public string Path { get; set; }
        }
    }
}
