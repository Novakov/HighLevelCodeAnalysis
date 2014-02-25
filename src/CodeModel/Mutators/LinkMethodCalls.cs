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

            var executionPaths = new ControlFlow().AnalyzeMethod(node.Method).FindPaths();

            var calls = new List<Tuple<Instruction, PotentialType[]> >();

            Parallel.ForEach(executionPaths, executionPath =>
            {
                var parameters = new DetermineCallParameterTypes();
                parameters.Walk(node.Method, executionPath);

                calls.AddRange(parameters.Calls);
            });

            foreach (var call in calls.GroupBy(x => (MethodInfo)x.Item1.Operand))
            {
                var link = new MethodCallLink(call.Key.GetGenericArguments(), call.Select(x => x.Item2).Distinct(new ArrayComparer<PotentialType>()).ToArray());

                LinkToMethod(node, context, call.Key, link);
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