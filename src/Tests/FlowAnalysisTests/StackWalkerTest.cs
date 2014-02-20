using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CodeModel.Extensions.DgmlExport;
using CodeModel.FlowAnalysis;
using NUnit.Framework;
using TestTarget;

namespace Tests.FlowAnalysisTests
{
    [TestFixture]
    public class StackWalkerTest
    {
        [Test]
        public void StackLengthAtTheEndOfMethodShouldBeZero()
        {
            IEnumerable<MethodInfo> methods = typeof(ControlFlowAnalysisTarget).GetMethods().Where(x => x.Name.StartsWith("Method"));

            foreach (var method in methods)
            {
                var graph = new ControlFlow().AnalyzeMethod(method);

                var branches = graph.FindPaths().ToList();

                foreach (var branch in branches)
                {
                    var stackLength = branch.SelectMany(x => x.Instructions).Aggregate(0, (a, i) => a + i.PushedValuesCount(method) - i.PopedValuesCount(method));

                    Assert.That(stackLength, Is.EqualTo(0));
                }
            }
        }       

        [Test, Explicit]
        [TestCaseSource(typeof(AllMscorlibTypes), "AllTypes")]
        public void CheckStackLength(Type type)
        {
            IEnumerable<MethodInfo> methods = type.GetMethods().Where(x => x.GetMethodBody() != null).Where(x => x.DeclaringType == type);

            foreach (var method in methods)
            {
                var graph = new ControlFlow().AnalyzeMethod(method);

                var branches = graph.FindPaths().ToList();

                foreach (var branch in branches)
                {
                    var stackLength = branch.SelectMany(x => x.Instructions).Aggregate(0, (a, i) => a + i.PushedValuesCount(method) - i.PopedValuesCount(method));

                    Assert.That(stackLength, Is.EqualTo(0), string.Format("Type: {0} Method:{1}", method, method.DeclaringType));
                }
            }
        }        
    }

    internal class AllMscorlibTypes
    {
        public IEnumerable<Type> AllTypes()
        {
            return typeof (string).Assembly.GetTypes()
                .Where(x => x.IsPublic && !x.IsInterface)
                .Where(x => x != typeof (StringBuilder))
                .OrderBy(x => x.FullName);
        }
    }
}
