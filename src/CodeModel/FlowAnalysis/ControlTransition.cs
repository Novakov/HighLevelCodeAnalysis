using CodeModel.Graphs;

namespace CodeModel.FlowAnalysis
{
    public class ControlTransition : Link
    {
        public TransitionKind Kind { get; private set; }

        public ControlTransition(TransitionKind kind)
        {
            this.Kind = kind;
        }
    }
}