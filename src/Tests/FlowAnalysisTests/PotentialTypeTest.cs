using System;
using CodeModel.FlowAnalysis;
using NUnit.Framework;

namespace Tests.FlowAnalysisTests
{
    [TestFixture]
    public class PotentialTypeTest
    {
        [Test]
        [TestCase(typeof(decimal), typeof(decimal), typeof(decimal))]        
        [TestCase(typeof(decimal), typeof(UInt64), typeof(decimal))]
        [TestCase(typeof(decimal), typeof(Int64), typeof(decimal))]
        [TestCase(typeof(decimal), typeof(UInt32), typeof(decimal))]
        [TestCase(typeof(decimal), typeof(Int32), typeof(decimal))]
        [TestCase(typeof(decimal), typeof(UInt16), typeof(decimal))]
        [TestCase(typeof(decimal), typeof(Int16), typeof(decimal))]
        [TestCase(typeof(decimal), typeof(Byte), typeof(decimal))]
        [TestCase(typeof(decimal), typeof(sbyte), typeof(decimal))]
        
        [TestCase(typeof(double), typeof(double), typeof(double))]
        [TestCase(typeof(double), typeof(float), typeof(double))]
        [TestCase(typeof(double), typeof(UInt64), typeof(double))]
        [TestCase(typeof(double), typeof(Int64), typeof(double))]
        [TestCase(typeof(double), typeof(UInt32), typeof(double))]
        [TestCase(typeof(double), typeof(Int32), typeof(double))]
        [TestCase(typeof(double), typeof(UInt16), typeof(double))]
        [TestCase(typeof(double), typeof(Int16), typeof(double))]
        [TestCase(typeof(double), typeof(Byte),  typeof(double))]
        [TestCase(typeof(double), typeof(sbyte), typeof(double))]

        [TestCase(typeof(float), typeof(float), typeof(float))]
        [TestCase(typeof(float), typeof(UInt64), typeof(float))]
        [TestCase(typeof(float), typeof(Int64), typeof(float))]
        [TestCase(typeof(float), typeof(UInt32), typeof(float))]
        [TestCase(typeof(float), typeof(Int32), typeof(float))]
        [TestCase(typeof(float), typeof(UInt16), typeof(float))]
        [TestCase(typeof(float), typeof(Int16), typeof(float))]
        [TestCase(typeof(float), typeof(Byte), typeof(float))]
        [TestCase(typeof(float), typeof(sbyte), typeof(float))]

        [TestCase(typeof(UInt64), typeof(UInt64), typeof(UInt64))]
        [TestCase(typeof(UInt64), typeof(UInt32), typeof(UInt64))]
        [TestCase(typeof(UInt64), typeof(UInt16), typeof(UInt64))]
        [TestCase(typeof(UInt64), typeof(Byte), typeof(UInt64))]
        
        [TestCase(typeof(Int64), typeof(Int64), typeof(Int64))]
        [TestCase(typeof(Int64), typeof(UInt32), typeof(Int64))]
        [TestCase(typeof(Int64), typeof(Int32), typeof(Int64))]
        [TestCase(typeof(Int64), typeof(UInt16), typeof(Int64))]
        [TestCase(typeof(Int64), typeof(Int16), typeof(Int64))]
        [TestCase(typeof(Int64), typeof(byte), typeof(Int64))]
        [TestCase(typeof(Int64), typeof(sbyte), typeof(Int64))]

        [TestCase(typeof(UInt32), typeof(UInt32), typeof(UInt32))]
        [TestCase(typeof(UInt32), typeof(Int32), typeof(Int64))]
        [TestCase(typeof(UInt32), typeof(UInt16), typeof(UInt32))]
        [TestCase(typeof(UInt32), typeof(Int16), typeof(Int64))]
        [TestCase(typeof(UInt32), typeof(Byte), typeof(UInt32))]
        [TestCase(typeof(UInt32), typeof(sbyte), typeof(Int64))]
        
        [TestCase(typeof(Int32), typeof(Int32), typeof(Int32))]
        [TestCase(typeof(Int32), typeof(UInt16), typeof(Int32))]
        [TestCase(typeof(Int32), typeof(Int16), typeof(Int32))]
        [TestCase(typeof(Int32), typeof(Byte), typeof(Int32))]
        [TestCase(typeof(Int32), typeof(SByte), typeof(Int32))]  
                
        [TestCase(typeof(UInt16), typeof(UInt16), typeof(Int32))]
        [TestCase(typeof(UInt16), typeof(Int16), typeof(Int32))]
        [TestCase(typeof(UInt16), typeof(Byte), typeof(Int32))]
        [TestCase(typeof(UInt16), typeof(SByte), typeof(Int32))] 
                
        [TestCase(typeof(Int16), typeof(Int16), typeof(Int32))]
        [TestCase(typeof(Int16), typeof(Byte), typeof(Int32))]
        [TestCase(typeof(Int16), typeof(SByte), typeof(Int32))]
                
        [TestCase(typeof(Byte), typeof(Byte), typeof(Int32))]
        [TestCase(typeof(Byte), typeof(SByte), typeof(Int32))]
        
        [TestCase(typeof(SByte), typeof(SByte), typeof(Int32))]
        public void ResultOfBinaryOperation(Type left, Type right, Type result)
        {
            var potentialLeft = PotentialType.Simple(left);
            var potentialRight = PotentialType.Simple(right);

            var expected = PotentialType.Simple(result);

            var actual1 = potentialLeft.AfterBinaryOperationWith(potentialRight);
            var actual2 = potentialRight.AfterBinaryOperationWith(potentialLeft);

            Assert.That(actual1, Is.EqualTo(expected), string.Format("{0} op {1} should give {2}", left, right, expected));
            Assert.That(actual2, Is.EqualTo(expected), string.Format("{1} op {0} should give {2}", left, right, expected));
        }
    }
}
