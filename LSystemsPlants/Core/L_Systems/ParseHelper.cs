using System;
using System.Collections.Generic;
using System.Linq;

namespace LSystemsPlants.Core.L_Systems
{
    public static class ParseHelper
    {
        private static Dictionary<char, Symbol> _charToSymbol = new Dictionary<char, Symbol>()
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

            foreach (var ruleString in rules.Where(r => !string.IsNullOrWhiteSpace(r)).Select(Clean))
            {
                var parts = ruleString.Split(new string[] {"->" }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length != 2 || string.IsNullOrWhiteSpace(parts[0]) || string.IsNullOrWhiteSpace(parts[1]))
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
            var result = new List<Symbol>();

            foreach (char c in axiom.Clean())
            {
                result.Add(c.GetReplacement());
            }

            return result;
        }


        public static Symbol GetReplacement(this char c)
        {
            return _charToSymbol[c];
        }


        public static string Clean(this string str)
        {
            return str.Trim().Replace(" ", "");
        }
    }
}
