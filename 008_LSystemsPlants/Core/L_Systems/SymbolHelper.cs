using System.Collections.Generic;
using System.Linq;

namespace LSystemsPlants.Core.L_Systems
{
    public static class SymbolHelper
    {
        private static readonly Dictionary<Symbol, string> _symbolsStrings = new Dictionary<Symbol, string>()
        {
            {Symbol.FORWARD_DRAW , "F"},
            {Symbol.FORWARD_NO_DRAW , "f"},
            {Symbol.POP_STATE , "]"},
            {Symbol.PUSH_STATE , "["},
            {Symbol.TURN_LEFT , "+"},
            {Symbol.TURN_RIGHT , "-"},
            {Symbol.L , "L"},
            {Symbol.R , "R"},
        };

        public static string GetRepresentation(this Symbol s)
        {
            return _symbolsStrings[s];
        }

        public static string GetString(params Symbol[] symbols)
        {
            return string.Concat(symbols.Select(s => s.GetRepresentation()));
        }
    }
}
