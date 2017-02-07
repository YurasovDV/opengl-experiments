using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSystemsPlants.Core.L_Systems
{
    struct Rule
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
