using Common;

namespace LSystemsPlants.Core.L_Systems
{
    public class ModelGenerator
    {
        public SimpleModel Generate(IGrammar g, GeneratorSettings settings)
        {
            var symbols = g.GenerateSequence(settings);
            var interpreter = GetInterpreter();
            return interpreter.GetModel(symbols);
        }

        protected TurtleInterpreter GetInterpreter()
        {
            var initialState = new TurtleState();

            initialState.Coordinates[0] = -400;
            initialState.Coordinates[1] = -400;

            var interpreter = new TurtleInterpreter(initialState);
            return interpreter;
        }
    }
}
