using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSystemsPlants.Core.L_Systems
{
    public class KochGrammar : BaseGrammar
    {
        public KochGrammar() : base()
        {
            Axiom = new[] 
            {
                Symbol.FORWARD_DRAW,
                Symbol.TURN_RIGHT,
                Symbol.FORWARD_DRAW,
                Symbol.TURN_RIGHT,
                Symbol.FORWARD_DRAW,
                Symbol.TURN_RIGHT,
                Symbol.FORWARD_DRAW,
            };

            var ruleSet = new List<Rule>()
            {
                // F=FF-
                // [-F+F+F]
                // +
                // [+F-F-F]
                new Rule(Symbol.FORWARD_DRAW, new[]
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
                })
            };

            RuleSet = ruleSet;
        }
    }
}
