using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSystemsPlants.Core.L_Systems
{
    interface IGrammar
    {
        IEnumerable<Symbol> GenerateSequence(GeneratorSettings settings);

    }
}
