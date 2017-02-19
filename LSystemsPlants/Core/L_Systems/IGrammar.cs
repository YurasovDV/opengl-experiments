using System.Collections.Generic;

namespace LSystemsPlants.Core.L_Systems
{
    interface IGrammar
    {
        IEnumerable<Symbol> GenerateSequence(GeneratorSettings settings);

    }
}
