using System.Collections.Generic;
using System.Linq;

namespace LSystemsPlants.Core.L_Systems
{
    public class Rule
    {
        public Rule(Symbol left, IEnumerable<Symbol> right)
        {
            Predecessor = left;
            Successor = right.ToArray();
        }

        public Symbol Predecessor { get; private set; }
        public Symbol[] Successor { get; private set; }
    }
}
