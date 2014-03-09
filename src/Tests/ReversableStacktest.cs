using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeModel;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class ReversableStackTest
    {
        [Test]
        [TestCase(1)]
        [TestCase(1, 2, 3, 4)]
        public void PushTest(object[] values)
        {
            var stack = new ReversableStack<int>();

            foreach (var i in values)
            {
                stack.Push((int)i);
            }

            Assert.That(stack, Is.EqualTo(values.Reverse()));
        }

        [Test]
        [TestCase(1)]
        [TestCase(1, 2, 3, 4)]
        public void PushManyTest(object[] values)
        {
            var stack = new ReversableStack<int>();

            stack.PushMany(values.OfType<int>());

            Assert.That(stack, Is.EqualTo(values.Reverse()));
        }

        [Test]
        [TestCase(1)]
        [TestCase(1, 2, 3, 4)]
        public void PopTest(object[] values)
        {
            var stack = new ReversableStack<int>();

            foreach (var i in values)
            {
                stack.Push((int)i);
            }

            for (int i = values.Length - 1; i >= 0; i--)
            {
                var fetched = stack.Pop();
                Assert.That(fetched, Is.EqualTo(values[i]));
            }
        }

        [Test]
        public void RevertToMark()
        {
            // arrange
            var stack = new ReversableStack<int>();
            stack.PushMany(1, 2, 3, 4);

            stack.Mark();

            stack.PushMany(7, 8, 9);

            // act
            stack.Revert();

            // assert
            Assert.That(stack, Is.EqualTo(new[] { 1, 2, 3, 4 }.Reverse()));
        }

        [Test]
        public void RevertToMultipleMarks()
        {
            // arrange
            var stack = new ReversableStack<int>();
            stack.PushMany(1, 2, 3, 4);

            stack.Mark();

            stack.PushMany(7, 8, 9);

            stack.Mark();

            stack.PushMany(10, 11, 12);

            // act & asserts
            stack.Revert();
            Assert.That(stack, Is.EqualTo(new[] {1, 2, 3, 4, 7, 8, 9}.Reverse()));

            stack.Revert();
            Assert.That(stack, Is.EqualTo(new[] { 1, 2, 3, 4 }.Reverse()));            

        }
    }
}
