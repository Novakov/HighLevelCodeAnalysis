using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CodeModel.Graphs;
using Mono.Reflection;

namespace CodeModel.FlowAnalysis
{
    public class DetermineCallParameterTypes : BaseCfgWalker<TypeAnalysisState>
    {
        public IDictionary<MethodBase, HashSet<PotentialType[]>> Calls { get { return this.recorder.Calls; } }

        private CallParameterTypesRecorder recorder;

        public void Walk(MethodInfo method, ControlFlowGraph graph)
        {
            this.recorder = new CallParameterTypesRecorder();
            this.recorder.Initialize(method);

            //FIXME: Remove graph cloning (and all supporting methods)
            var reducedGraph = graph.Clone();           

            base.WalkCore(method, reducedGraph);
        }
        
        protected override TypeAnalysisState VisitBlock(TypeAnalysisState alreadyExecutedCommands, BlockNode block)
        {
            return this.recorder.Visit(alreadyExecutedCommands, block.Instructions);
        }

        protected override TypeAnalysisState GetInitialState(MethodInfo method, ControlFlowGraph graph)
        {
            var variables = method.GetMethodBody().LocalVariables.ToDictionary(x => x.LocalIndex, x => PotentialType.FromType(x.LocalType));
            var parameters = method.GetParameters().ToDictionary(x => x.Position, x => PotentialType.FromType(x.ParameterType));

            return new TypeAnalysisState(variables, parameters);                
        }
    }
}
