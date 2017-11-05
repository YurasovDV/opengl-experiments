using System.Collections.Generic;
using System.Text;

namespace LSystemsPlants.Core.L_Systems
{
    public static class SymbolHelper
    {
        private static Dictionary<Symbol, string> _symbolsStrings = new Dictionary<Symbol, string> ()
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
            var res = new StringBuilder();

            foreach (var s in symbols)
            {
                res.Append(s.GetRepresentation());
            }

            return res.ToString();
        }
    }
}
