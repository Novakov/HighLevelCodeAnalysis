using CodeModel.Graphs;

namespace CodeModel.Model
{
    public class ApplicationEntryPoint : Node
    {
        public ApplicationEntryPoint() : base("ApplicationEntryPoint")
        {
        }

        public override string DisplayLabel
        {
            get { return "Application Entry Point"; }
        }
    }
}