using System;
using System.Collections.Generic;
using System.Linq;

namespace LSystemsPlants.Core.L_Systems
{
    public static class ParseHelper
    {
        private static readonly Dictionary<char, Symbol> _charToSymbol = new Dictionary<char, Symbol>()
        {
            {'F', Symbol.FORWARD_DRAW },
            {'f', Symbol.FORWARD_NO_DRAW },
            {']', Symbol.POP_STATE },
            {'[', Symbol.PUSH_STATE },
            {'+', Symbol.TURN_LEFT },
            {'-', Symbol.TURN_RIGHT },
            {'L', Symbol.L },
            {'R', Symbol.R },
        };


        public static IGrammar GetGrammar(string axiom, string[] rules)
        {
            IEnumerable<Symbol> axParsed = Parse(axiom);

            var rulesParsed = new List<Rule>();

            foreach (var ruleString in rules.Where(r => r.HasValue()).Select(RemoveSpaces))
            {
                var parts = ruleString.Split(new string[] { "->" }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length != 2 || !parts[0].HasValue() || !parts[1].HasValue())
                {
                    continue;
                }

                IEnumerable<Symbol> pred = Parse(parts[0]);
                IEnumerable<Symbol> succ = Parse(parts[1]);

                var rule = new Rule(pred.Single(), succ);
                rulesParsed.Add(rule);
            }

            return new SimplestGrammar(rulesParsed, axParsed);
        }

        private static IEnumerable<Symbol> Parse(string axiom)
        {
            return axiom.RemoveSpaces().Select(c => c.GetReplacement()).ToList();
        }

        public static Symbol GetReplacement(this char c) => _charToSymbol[c];

        public static string RemoveSpaces(this string str) => str.Trim().Replace(" ", "");

        public static bool HasValue(this string str) => !string.IsNullOrWhiteSpace(str);
    }
}
