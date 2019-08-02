using Common;
using System;
using System.Collections.Generic;

namespace LSystemsPlants.Core.L_Systems
{
    public class ModelGenerator
    {
        private readonly Dictionary<Type, TurtleState> states = new Dictionary<Type, TurtleState>()
        {
            {  typeof(SquareGrammar),new TurtleState(-400, -400)},
            {  typeof(TriangleGrammar),new TurtleState(-400, -400)},
            {  typeof(KochGrammar),new TurtleState(0, -400)},
            {  typeof(SimplestGrammar),new TurtleState(0, -400)},
            {  typeof(DragoGrammar),new TurtleState(0, 100)},
        };

        public SimpleModel Generate(IGrammar g, GeneratorSettings settings)
        {
            var symbols = g.GenerateSequence(settings);
            var interpreter = GetInterpreter(g);
            return interpreter.GetModel(symbols);
        }

        protected TurtleInterpreter GetInterpreter(IGrammar g)
        {
            if (!states.TryGetValue(g.GetType(), out TurtleState initialState))
            {
                initialState = new TurtleState();
            }

            var interpreter = new TurtleInterpreter(initialState);
            return interpreter;
        }
    }
}
