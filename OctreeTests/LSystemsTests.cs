using System;
using System.Linq;
using LSystemsPlants.Core.L_Systems;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OctreeTests
{
    [TestClass]
    public class LSystemsTests
    {
        [TestMethod]
        public void TestKoch()
        {
            var grammar = new KochGrammar();

            var seq = grammar.GenerateSequence(new GeneratorSettings() { MaxIteration = 1 }).ToArray();

            var part = new[]
            {
                    Symbol.FORWARD_DRAW,// F
                    Symbol.TURN_RIGHT, // -
                    Symbol.FORWARD_DRAW,// F
                    Symbol.TURN_LEFT, // +
                    Symbol.FORWARD_DRAW,// F
                    Symbol.TURN_RIGHT,// -
                    Symbol.FORWARD_DRAW,// F
                    Symbol.TURN_RIGHT,// -
                    Symbol.FORWARD_DRAW,// F

                    Symbol.TURN_RIGHT,// -

                    Symbol.FORWARD_DRAW,// F
                    Symbol.TURN_RIGHT, // -
                    Symbol.FORWARD_DRAW,// F
                    Symbol.TURN_LEFT, // +
                    Symbol.FORWARD_DRAW,// F
                    Symbol.TURN_RIGHT,// -
                    Symbol.FORWARD_DRAW,// F
                    Symbol.TURN_RIGHT,// -
                    Symbol.FORWARD_DRAW,// F

                    Symbol.TURN_RIGHT,// -

                    Symbol.FORWARD_DRAW,// F
                    Symbol.TURN_RIGHT, // -
                    Symbol.FORWARD_DRAW,// F
                    Symbol.TURN_LEFT, // +
                    Symbol.FORWARD_DRAW,// F
                    Symbol.TURN_RIGHT,// -
                    Symbol.FORWARD_DRAW,// F
                    Symbol.TURN_RIGHT,// -
                    Symbol.FORWARD_DRAW,// F

                    Symbol.TURN_RIGHT,// -

                    Symbol.FORWARD_DRAW,// F
                    Symbol.TURN_RIGHT, // -
                    Symbol.FORWARD_DRAW,// F
                    Symbol.TURN_LEFT, // +
                    Symbol.FORWARD_DRAW,// F
                    Symbol.TURN_RIGHT,// -
                    Symbol.FORWARD_DRAW,// F
                    Symbol.TURN_RIGHT,// -
                    Symbol.FORWARD_DRAW,// F
            };

            Assert.IsTrue(seq.Length == part.Length, "len");

            CollectionAssert.AreEquivalent(part, seq, "eq");

        }
    }
}
