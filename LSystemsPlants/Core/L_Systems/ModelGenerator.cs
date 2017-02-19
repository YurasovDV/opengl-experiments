using Common;

namespace LSystemsPlants.Core.L_Systems
{
    class ModelGenerator
    {
        public SimpleModel Generate()
        {
            var g = new SimplestGrammar();

            var symbols = g.GenerateSequence(new GeneratorSettings() { MaxIteration = Constants.Iterations });

            var initialState = new TurtleState();

            initialState.Coordinates[0] = -100;
            initialState.Coordinates[1] = -100;

            var interpreter = new TurtleInterpreter(initialState);

            return interpreter.GetModel(symbols);
        }
    }
}
