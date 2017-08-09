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
        public void Koch()
        {
            var grammar = new KochGrammar();

            SymbolState[] generated = grammar.GenerateSequence(new GeneratorSettings() { MaxIteration = 1 }).ToArray();

            Symbol[] expected = new[]
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

            Assert.IsTrue(generated.Length == expected.Length, "len");

            CollectionAssert.AreEquivalent(expected, generated.Select(state => state.Symbol).ToArray(), "eq");

        }
    }
}
