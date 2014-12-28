using CodeModel.Graphs;

namespace CodeModel.Primitives
{
    public class ApplicationEntryPoint : Node
    {
        public const string NodeId = "ApplicationEntryPoint";

        public ApplicationEntryPoint() : base(NodeId)
        {
        }

        public override string DisplayLabel
        {
            get { return "Application Entry Point"; }
        }
    }
}