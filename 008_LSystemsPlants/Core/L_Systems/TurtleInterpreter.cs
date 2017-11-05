using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using OpenTK;

namespace LSystemsPlants.Core.L_Systems
{
    public class TurtleInterpreter
    {
        private TurtleState State { get; set; }
        private Stack<TurtleState> StateStack { get; set; }
        private List<Vector3> Vertices { get; set; }
        private List<Vector3> Colors { get; set; }

        public TurtleInterpreter(TurtleState initialState)
        {
            State = initialState;
            StateStack = new Stack<TurtleState>();
        }

        public SimpleModel GetModel(IEnumerable<SymbolState> symbols)
        {
            var model = new SimpleModel();

            Vertices = new List<Vector3>(symbols.Count());
            Colors = new List<Vector3>(symbols.Count());

            foreach (SymbolState symbol in symbols)
            {
                Execute(symbol);
            }

            model.Vertices = Vertices.ToArray();
            var red = new Vector3(Vector3.UnitX);
            model.Colors = Colors.ToArray();
            model.Normals = new Vector3[] { new Vector3() };
            Vertices = null;
            Colors = null;
            return model;
        }

        private void Execute(SymbolState symbol)
        {
            switch (symbol.Symbol)
            {
                case Symbol.L:
                case Symbol.R:
                case Symbol.FORWARD_DRAW:

                    Vertices.Add(new Vector3(State.Coordinates[0], State.Coordinates[1], State.Coordinates[2]));
                    State = Forward(State, symbol);
                    Vertices.Add(new Vector3(State.Coordinates[0], State.Coordinates[1], State.Coordinates[2]));

                    Colors.Add(symbol.Color);
                    Colors.Add(symbol.Color);

                    break;

                case Symbol.TURN_LEFT:
                    UpdateAngle(symbol.Delta);
                    break;

                case Symbol.TURN_RIGHT:
                    UpdateAngle(-symbol.Delta);
                    break;

                case Symbol.FORWARD_NO_DRAW:
                    State = Forward(State, symbol);
                    break;

                case Symbol.PUSH_STATE:
                    StateStack.Push(State);
                    break;

                case Symbol.POP_STATE:
                    State = StateStack.Pop();
                    break;

                default:
                    break;
            }
        }

        private TurtleState Forward(TurtleState state, SymbolState symbol)
        {
            Vector3 before = new Vector3(state.Coordinates[0], state.Coordinates[1], state.Coordinates[2]);

            var scaleMat = Matrix4.CreateScale(symbol.Step);

            var transformed = Vector3.Transform(Vector3.Transform(Vector3.UnitY, scaleMat), state.RotationMatrix) + before;

            return new TurtleState()
            {
                Coordinates = new[] 
                {
                    transformed.X,
                    transformed.Y,
                    transformed.Z
                },
                Angle = state.Angle,
                RotationMatrix = state.RotationMatrix
            };
        }

        private void UpdateAngle(float delta)
        {
            State.Angle += delta;

            // prevent accuracy loss
            if (Math.Abs(State.Angle) > 10)
            {
                State.Angle = State.Angle % MathHelper.TwoPi;
            }

            State.RotationMatrix = Matrix4.CreateRotationZ(State.Angle);
        }

    }
}
