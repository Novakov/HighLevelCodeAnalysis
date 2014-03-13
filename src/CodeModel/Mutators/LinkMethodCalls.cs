using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading.Tasks;
using CodeModel.Builder;
using CodeModel.FlowAnalysis;
using CodeModel.Graphs;
using CodeModel.Links;
using CodeModel.Model;
using Mono.Reflection;

namespace CodeModel.Mutators
{
    public class LinkMethodCalls : INodeMutator<MethodNode>
    {
        public void Mutate(MethodNode node, IMutateContext context)
        {
            var body = node.Method.GetMethodBody();

            if (body == null)
            {
                return;
            }

            var cfg = new ControlFlow().AnalyzeMethod(node.Method);

            var tmp = new DetermineCallParameterTypes();

            tmp.Walk(node.Method, cfg);

            foreach (var call in tmp.Calls.GroupBy(x => (MethodBase)x.Item1.Operand))
            {
                if (!(call.Key is MethodInfo))
                {
                    continue;                    
                }

                var link = new MethodCallLink(call.Key.GetGenericArguments(), call.Select(x => x.Item2).Distinct(new ArrayComparer<PotentialType>()).ToArray());

                LinkToMethod(node, context, (MethodInfo) call.Key, link);
            }
        }

        private static void LinkToMethod(MethodNode node, IMutateContext context, MethodInfo calledMethod, MethodCallLink link)
        {
            var targetMethod = calledMethod;

            if (calledMethod.IsGenericMethod)
            {
                targetMethod = calledMethod.GetGenericMethodDefinition();
            }

            var targetNode = context.FindNodes<MethodNode>(x => x.Method == targetMethod).SingleOrDefault();

            if (targetNode != null)
            {
                context.AddLink(node, targetNode, link);
            }
        }
    }
}