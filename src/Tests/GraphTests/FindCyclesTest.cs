using System;
using System.Collections.Generic;
using System.Linq;
using CodeModel.Graphs;
using NUnit.Framework;
using Tests.EntryPointTests;

namespace Tests.GraphTests
{
    public class FindCyclesTest : IHaveGraph<FindCyclesTest.SampleNode, FindCyclesTest.SampleLink>
    {
        public Graph<FindCyclesTest.SampleNode, FindCyclesTest.SampleLink> Result { get; private set; }

        [Test]
        [TestCase("AB,BA", new[] { "A,B" })]
        [TestCase("AB,BC,CA", new[] { "A,B,C" })]
        [TestCase("AB,BC,CA,DE,EF,FD", new[] { "A,B,C", "D,E,F" })]
        [TestCase("AB,BC,CA,DE,EF,FD,BD", new[] { "A,B,C", "D,E,F" })]
        [TestCase("AB,BC,CD,DE", new string[0])]
        [TestCase("AB,BC,CA,BD,DE,EF,FC", new[] { "A,B,C", "A,B,D,E,F,C" })]
        //[TestCase("AB,AC,CF,BF,FA,AG,FD,BG,BD,GD,GE,EB,DH,EH,HA,GC,CJ,KC,KJ,IK,JI,IG,HI,HG", 
        //    new[] { "A,B,F", "A,B,D,F,A", "B,D,H,E", "A,B,D,H,G", "A,F,C", "A,G,C,F", "C,J,I,G" }
        //)] // nice try ;)
        public void ShouldFindProperCycles(string graphSpec, string[] expectedCycles)
        {
            // arrange
            this.Result = new Graph<SampleNode, SampleLink>();
            var nodes = graphSpec.Where(x => x != ',').Distinct().ToDictionary(x => x.ToString(), x => new SampleNode(x.ToString()));

            foreach (var node in nodes)
            {
                this.Result.AddNode(node.Value);
            }

            foreach (var linkSpec in graphSpec.Split(','))
            {
                var source = nodes[linkSpec[0].ToString()];
                var target = nodes[linkSpec[1].ToString()];

                this.Result.AddLink(source, target, new SampleLink());
            }

            var finder = new FindCycles<SampleNode, SampleLink>();

            // act
            var foundCycles = finder.Find(this.Result);

            // assert            
            Assert.That(foundCycles.Select(x => x.Select(y => y.Id)), Is
                .EquivalentTo(expectedCycles.Select(x => x.Split(',')))
                );
        }

        public class SampleNode : Node
        {
            public SampleNode(string nodeId)
                : base(nodeId)
            {
            }
        }

        public class SampleLink : Link
        {
        }
    }
}
