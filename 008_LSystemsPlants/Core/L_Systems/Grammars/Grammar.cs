using System.Collections.Generic;

namespace LSystemsPlants.Core.L_Systems
{
    class SimplestGrammar : BaseGrammar
    {
        public SimplestGrammar(IEnumerable<Rule> ruleSet, IEnumerable<Symbol> axiom)
        {
            RuleSet = ruleSet;
            Axiom = axiom;
        }

        public SimplestGrammar()
        {
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
                    Symbol.PUSH_STATE, // [
                    Symbol.TURN_RIGHT,// -
                    Symbol.FORWARD_DRAW,// F
                    Symbol.TURN_LEFT, // +
                    Symbol.FORWARD_DRAW,// F
                    Symbol.TURN_LEFT, // +
                    Symbol.FORWARD_DRAW,// F
                    Symbol.POP_STATE, // ]
                    Symbol.TURN_LEFT, // +
                    Symbol.PUSH_STATE, // [
                    Symbol.TURN_LEFT, // +
                    Symbol.FORWARD_DRAW,// F
                    Symbol.TURN_RIGHT,// -
                    Symbol.FORWARD_DRAW,// F
                    Symbol.TURN_RIGHT,// -
                    Symbol.FORWARD_DRAW,// F
                    Symbol.POP_STATE, // ]
                })
            };

            RuleSet = ruleSet;
            Axiom = new[] { Symbol.FORWARD_DRAW };
        }
    }
}
