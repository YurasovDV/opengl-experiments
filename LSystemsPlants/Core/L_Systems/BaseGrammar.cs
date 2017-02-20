using System;
using System.Collections.Generic;
using System.Linq;

namespace LSystemsPlants.Core.L_Systems
{
    public class BaseGrammar : IGrammar
    {
        protected IEnumerable<Rule> RuleSet { get; set; }
        protected IEnumerable<Symbol> Axiom { get; set; }

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
                    else
                    {
                        current.Add(symbol);
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
