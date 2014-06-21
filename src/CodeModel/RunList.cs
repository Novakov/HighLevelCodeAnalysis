﻿using System.Collections.Generic;
using System.Linq;

namespace CodeModel
{
    public class RunList<TElement>
    {
        public IEnumerable<TElement> Elements { get; private set; }
        public string[] Missing { get; private set; }
        public bool IsValid { get; private set; }

        public RunList(IEnumerable<TElement> elements, string[] missing)
        {
            this.Elements = elements;
            this.Missing = missing;
            this.IsValid = !missing.Any();
        }
    }
}