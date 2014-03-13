using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeModel;
using NUnit.Framework;
using TestTarget.Implementing;

namespace Tests
{
    [TestFixture]
    public class ReflectionExtensionsTest
    {
        [Test]
        public void GetImplementedMethodsShouldReturnProperValues()
        {
            var implementingMethod = Get.MethodOf<ImplementingType>(x => x.ImplicitMethod());
            var implementedMethod = Get.MethodOf<IInterface>(x => x.ImplicitMethod());

            var implementedMethods = implementingMethod.GetImplementedMethod(typeof(IInterface));

            Assert.That(implementedMethods, Is.EqualTo(implementedMethod));
        }
    }
}
