using Common;

namespace LSystemsPlants.Core.L_Systems
{
    class ModelGenerator
    {
        public SimpleModel Generate(IGrammar g)
        {
            var symbols = g.GenerateSequence(new GeneratorSettings() { MaxIteration = Constants.Iterations });

            var initialState = new TurtleState();

            initialState.Coordinates[0] = -200;
            initialState.Coordinates[1] = -200;

            var interpreter = new TurtleInterpreter(initialState);

            return interpreter.GetModel(symbols);
        }
    }
}
