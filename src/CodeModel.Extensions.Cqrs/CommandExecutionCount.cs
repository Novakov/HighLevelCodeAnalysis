namespace CodeModel.Extensions.Cqrs
{
    public class CommandExecutionCount
    {
        public int HighestCount { get; private set; }

        public CommandExecutionCount(int highestCount)
        {
            this.HighestCount = highestCount;
        }
    }
}