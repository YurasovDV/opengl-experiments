using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;

namespace LSystemsPlants.Core.L_Systems
{
    public class BaseGrammar : IGrammar
    {
        protected IEnumerable<Rule> RuleSet { get; set; }
        protected IEnumerable<Symbol> Axiom { get; set; }

        public int RulesCount
        {
            get
            {
                return RuleSet.Count();
            }
        }

        public virtual GeneratorSettings DefaultSettings
        {
            get
            {
                return GeneratorSettings.Default;
            }
        }

        public string GetAxiom()
        {
            return SymbolHelper.GetString(Axiom.ToArray());
        }

        /// <summary>
        /// from zero
        /// </summary>
        /// <param name="ruleNumber"></param>
        /// <returns></returns>
        public string GetRule(int ruleNumber)
        {
            var rule = RuleSet.ElementAtOrDefault(ruleNumber);
            if (rule == null)
            {
                throw new IndexOutOfRangeException($"index was {ruleNumber}");
            }

            return $"{SymbolHelper.GetString(rule.Predecessor)} -> {SymbolHelper.GetString(rule.Successor)}";
        }

        public IEnumerable<SymbolState> GenerateSequence(GeneratorSettings settings)
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

            var delta = settings.InitialDelta;
            var step = settings.InitialStep;
            var color = Vector3.UnitX;
            var initialColor = new Vector3(1, 0, 1);

            SymbolState symbolToState(Symbol s) => new SymbolState()
            {
                Delta = delta,
                Step = step,
                Symbol = s,
                Color = color
            };

            var current = new List<SymbolState>();
            var previous = Axiom
                .Select(s => new SymbolState()
                {
                    Delta = delta,
                    Step = step,
                    Symbol = s,
                    Color = color
                })
                .ToList();

            for (int iter = 0; iter < maxIteration; iter++)
            {
                foreach (var symbolState in previous)
                {
                    IEnumerable<Symbol> replacement = GetReplacement(symbolState.Symbol);
                    if (replacement.Any())
                    {
                        current.AddRange(replacement.Select(symbolToState));
                    }
                    else
                    {
                        current.Add(symbolState);
                    }
                }

                float redFade = 1 - (iter / (float)maxIteration);
                float blueAppear = 1.0f - redFade;

                color = new Vector3(initialColor.X * redFade, initialColor.Y, blueAppear * initialColor.Z);
                previous = current;
                current = new List<SymbolState>(previous.Count * 2);

                step *= settings.StepChangeAtEveryLevel;
                delta *= settings.DeltaChangeAtEveryLevel;
            }

            IEnumerable<SymbolState> result = previous;
            return result;
        }

        private IEnumerable<Symbol> GetReplacement(Symbol symbol)
        {
            return
                RuleSet.FirstOrDefault(r => r.Predecessor == symbol)
                    ?.Successor
                    ?.ToArray()
                ?? new Symbol[0];
        }
    }
}
