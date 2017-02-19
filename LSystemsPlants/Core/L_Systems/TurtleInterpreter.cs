using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using OpenTK;

namespace LSystemsPlants.Core.L_Systems
{
    public class TurtleInterpreter
    {
        public float StepSize { get; set; }

        private TurtleState State { get; set; }
        private Stack<TurtleState> StateStack { get; set; }
        private List<Vector3> Vertices { get; set; }

        public TurtleInterpreter(TurtleState initialState)
        {
            State = initialState;
            StateStack = new Stack<TurtleState>();
            StepSize = Constants.DefaultStep;
        }

        public SimpleModel GetModel(IEnumerable<Symbol> symbols)
        {
            var model = new SimpleModel();

            Vertices = new List<Vector3>(symbols.Count());

            foreach (var symbol in symbols)
            {
                Execute(symbol);
            }

            model.Vertices = Vertices.ToArray();
            var red = new Vector3(Vector3.UnitX);
            model.Colors = Enumerable.Repeat(red, Vertices.Count).ToArray();
            model.Normals = Enumerable.Repeat(red, Vertices.Count).ToArray();
            Vertices = null;
            return model;
        }

        private void Execute(Symbol symbol)
        {
            switch (symbol)
            {
                case Symbol.FORWARD_DRAW:

                    Vertices.Add(new Vector3(State.Coordinates[0], State.Coordinates[1], State.Coordinates[2]));
                    State = Forward(State);
                    Vertices.Add(new Vector3(State.Coordinates[0], State.Coordinates[1], State.Coordinates[2]));

                    break;

                case Symbol.TURN_LEFT:
                    UpdateAngle(Constants.Delta);
                    break;

                case Symbol.TURN_RIGHT:
                    UpdateAngle(-Constants.Delta);
                    break;

                case Symbol.FORWARD_NO_DRAW:
                    State = Forward(State);
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

        private TurtleState Forward(TurtleState state)
        {
            Vector3 before = new Vector3(state.Coordinates[0], state.Coordinates[1], state.Coordinates[2]);

            var rotMat = Matrix4.CreateRotationZ(state.Angle);
            var scaleMat = Matrix4.CreateScale(StepSize);

            var transformed = Vector3.Transform(Vector3.Transform(Vector3.UnitY, scaleMat), rotMat) + before;

            return new TurtleState()
            {
                Coordinates = new[] 
                {
                    transformed.X,
                    transformed.Y,
                    transformed.Z
                },
                Angle = state.Angle
            };
        }

        private void UpdateAngle(float delta)
        {
            State.Angle += delta;
        }
    }
}
