﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTarget
{
    public class MemberAccess
    {
        public string ThisProperty { get; set; }

        public string ThisField;

        public string PublicPropertyWithPrivateSetter { get; private set; }

        public void Access()
        {
            this.ThisField = "aaaa";

            if (this.ThisField.Length == 4)
            {
                throw new Exception("aaaaa");
            }

            this.ThisProperty = "aaaa";

            if (this.ThisProperty.Length == 4)
            {
                throw new Exception("aaaaa");
            }

            this.PublicPropertyWithPrivateSetter = "4";
        }
    }
}
