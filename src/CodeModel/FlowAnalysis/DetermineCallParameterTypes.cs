using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CodeModel.FlowAnalysis
{
    public class DetermineCallParameterTypes
    {
        public IDictionary<MethodBase, HashSet<PotentialType[]>> Calls { get { return this.recorder.Calls; } }

        private CallParameterTypesRecorder recorder;

        public void Walk(MethodInfo method, ControlFlowGraph graph)
        {
            this.recorder = new CallParameterTypesRecorder();
            
            this.recorder.Initialize(method);

            var variables = method.GetMethodBody().LocalVariables.ToDictionary(x => x.LocalIndex, x => PotentialType.FromType(x.LocalType));
            var parameters = method.GetParameters().ToDictionary(x => x.Position, x => PotentialType.FromType(x.ParameterType));
            var initialState = new TypeAnalysisState(variables, parameters);

            var walker = new ControlFlowGraphWalker<TypeAnalysisState>
            {
                InitialState = initialState,
                VisitingBlock = (state, block) => this.recorder.Visit(state, block.Instructions)
            };

            walker.WalkCore(method, graph);
        }
    }
}
