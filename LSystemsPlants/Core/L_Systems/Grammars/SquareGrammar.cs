using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSystemsPlants.Core.L_Systems
{
    public class SquareGrammar : BaseGrammar
    {
        public override GeneratorSettings DefaultSettings
        {
            get
            {
                return new GeneratorSettings()
                {
                    DeltaChangeAtEveryLevel = 1,
                    InitialDelta = 1.570796f,
                    InitialStep = 40,
                    MaxIteration = 5,
                    StepChangeAtEveryLevel = 0.5f
                };
            }
        }

        public SquareGrammar() : base()
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
                    Symbol.FORWARD_DRAW,// F
                    Symbol.TURN_RIGHT, // -
                    Symbol.FORWARD_DRAW,// F
                    Symbol.TURN_RIGHT, // -
                    Symbol.FORWARD_DRAW, // F
                    Symbol.TURN_RIGHT, // -
                    Symbol.FORWARD_DRAW, // F
                    Symbol.TURN_RIGHT, // -
                    Symbol.FORWARD_DRAW, // F
                    Symbol.FORWARD_DRAW, // F
                   
                })
            };

            RuleSet = ruleSet;
        }
    }
}
