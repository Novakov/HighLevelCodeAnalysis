using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeModel;
using NUnit.Framework.Constraints;

namespace Tests.Constraints
{
    public static class Annonated
    {
        public static IResolveConstraint With<TAnnotation>()
        {
            return new AnnonatedWithConstraint<TAnnotation>();
        }
    }

    public class AnnonatedWithConstraint<TAnnotation> : Constraint
    {       
        public override bool Matches(object actual)
        {
            this.actual = actual;

            return ((IAnnotable) actual).HasAnnotation<TAnnotation>();
        }

        public override void WriteDescriptionTo(MessageWriter writer)
        {
            writer.Write("{0} to be annonated with {1}", this.actual, typeof(TAnnotation).Name);            
        }        
    }
}
