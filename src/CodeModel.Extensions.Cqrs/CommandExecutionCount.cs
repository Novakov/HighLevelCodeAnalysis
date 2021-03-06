﻿using CodeModel.Graphs;

namespace CodeModel.Extensions.Cqrs
{
    public class CommandExecutionCount
    {
        [Exportable]
        public int HighestCount { get; private set; }

        public CommandExecutionCount(int highestCount)
        {
            this.HighestCount = highestCount;
        }
    }
}