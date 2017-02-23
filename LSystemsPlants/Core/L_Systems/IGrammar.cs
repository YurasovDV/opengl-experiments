using System.Collections.Generic;

namespace LSystemsPlants.Core.L_Systems
{
    public interface IGrammar
    {
        IEnumerable<SymbolState> GenerateSequence(GeneratorSettings settings);

        GeneratorSettings DefaultSettings { get;}

        int RulesCount { get; }
        string GetAxiom();
        string GetRule(int ruleNumber);
    }
}
