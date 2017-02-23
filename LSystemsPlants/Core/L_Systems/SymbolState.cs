using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace LSystemsPlants.Core.L_Systems
{
    public class SymbolState
    {
        public Symbol Symbol { get; set; }

        public float Delta { get; set; }

        public float Step { get; set; }

        public Vector3 Color { get; set; }
    }
}
