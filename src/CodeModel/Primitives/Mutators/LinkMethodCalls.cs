﻿using System.Linq;
using System.Reflection;
using CodeModel.Builder;
using CodeModel.Dependencies;
using CodeModel.FlowAnalysis;

namespace CodeModel.Primitives.Mutators
{
    [Provide(Resources.MethodCallLinks)]
    [Need(Resources.Methods)]
    public class LinkMethodCalls : INodeMutator<MethodNode>
    {
        public void Mutate(MethodNode node, IMutateContext context)
        {
            var body = node.Method.GetMethodBody();

            if (body == null)
            {
                return;
            }

            var cfg = ControlFlowGraphFactory.BuildForMethod(node.Method);

            var tmp = new DetermineCallParameterTypes();

            tmp.Walk(node.Method, cfg);

            foreach (var call in tmp.Calls)
            {
                if (!(call.Key is MethodInfo))
                {
                    continue;                    
                }

                var link = new MethodCallLink(call.Key.GetGenericArguments(), call.Value.ToArray());

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

            var targetNode = context.LookupNode<MethodNode>(MethodNode.IdFor(targetMethod));

            if (targetNode != null)
            {
                context.AddLink(node, targetNode, link);
            }
        }
    }
}