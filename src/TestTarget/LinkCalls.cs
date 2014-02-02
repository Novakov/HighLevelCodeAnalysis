using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTarget
{
    public class LinkCalls
    {
        public void Source()
        {
            this.NormalCall();
            this.GenericMethodCall<int>();
        }

        public void IndirectParameterTypesInSingleBranch()
        {
            var param = new Inherited();

            MethodWithBaseType(param);
        }

        public void IndirectParameterTypesInMultipleBranches()
        {
            Base value;

            if (DateTime.Today.Day == 23)
            {
                value = new Inherited(2);                
            }
            else
            {
                value = new Inherited(7);
            }

            MethodWithBaseType(value);
        }

        public void IndirectDifferentParameterTypesInMultipleBranches()
        {
            Base value;

            if (DateTime.Today.Day == 23)
            {
                value = new Inherited(2);
            }
            else
            {
                value = new SecondInherited();
            }

            MethodWithBaseType(value);
        }

        public void MultipleCallsWithTheSameParameters()
        {
            MethodWithBaseType(new Inherited(1));
            MethodWithBaseType(new Inherited(2));
        }

        public void MultipleCallsWithDifferentParameters()
        {
            MethodWithBaseType(new Inherited(2));
            MethodWithBaseType(new SecondInherited());
        }

        public void MethodWithBaseType(Base obj)
        {
            
        }

        public void GenericMethodCall<T>()
        {
            
        }

        public void NormalCall()
        {
            
        }
    }   
}
