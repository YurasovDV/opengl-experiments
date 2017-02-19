using System;
using System.Collections.Generic;
using System.Linq;

namespace LSystemsPlants.Core.L_Systems
{
    class SimplestGrammar : IGrammar
    {
        private IEnumerable<Rule> RuleSet { get; set; }
        private IEnumerable<Symbol> Axiom { get; set; }

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



        public IEnumerable<Symbol> GenerateSequence(GeneratorSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            int maxIteration = settings.MaxIteration;
            if (maxIteration == 0)
            {
                throw new InvalidOperationException(string.Format("{0} == 0", nameof(maxIteration)));
            }

            var current = new List<Symbol>();
            var previous = new List<Symbol>(Axiom);

            for (int iter = 0; iter < maxIteration; iter++)
            {
                current.Clear();

                foreach (var symbol in previous)
                {
                    IEnumerable<Symbol> replacement = GetReplacement(symbol);
                    if (replacement.Any())
                    {
                        current.AddRange(replacement);
                    }
                }

                previous = new List<Symbol>(current);
            }

            IEnumerable<Symbol> result = current;

            return result;
        }

        private IEnumerable<Symbol> GetReplacement(Symbol symbol)
        {
            foreach (var rule in RuleSet)
            {
                if (rule.Predecessor == symbol)
                {
                    return rule.Successor.ToArray();
                }
            }

            return new Symbol[0];
        }
    }
}
