using Common;

namespace LSystemsPlants.Core.L_Systems
{
    public class ModelGenerator
    {
        public SimpleModel Generate(IGrammar g, GeneratorSettings settings)
        {
            var symbols = g.GenerateSequence(settings);
            var interpreter = GetInterpreter(g);
            return interpreter.GetModel(symbols);
        }

        protected TurtleInterpreter GetInterpreter(IGrammar g)
        {
            var initialState = new TurtleState();

            if (g.GetType() == typeof(SquareGrammar))
            {
                initialState.Coordinates[0] = -400;
                initialState.Coordinates[1] = -400;
            }

            if (g.GetType() == typeof(TriangleGrammar))
            {
                initialState.Coordinates[0] = -400;
                initialState.Coordinates[1] = -400;
            }

            if (g.GetType() == typeof(KochGrammar))
            {
                //initialState.Coordinates[0] = -400;
                initialState.Coordinates[1] = -400;
            }

            if (g.GetType() == typeof(SimplestGrammar))
            {
                //initialState.Coordinates[0] = -400;
                initialState.Coordinates[1] = -400;
            }

            var interpreter = new TurtleInterpreter(initialState);
            return interpreter;
        }
    }
}
