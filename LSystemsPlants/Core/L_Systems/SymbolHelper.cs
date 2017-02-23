using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSystemsPlants.Core.L_Systems;

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
