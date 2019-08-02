using OpenTK;

namespace LSystemsPlants.Core.L_Systems
{
    public class TurtleState
    {
        public Matrix4 RotationMatrix;

        public float[] Coordinates { get; set; }

        public float Angle { get; set; }

        public TurtleState(float x = 0, float y = 0)
        {
            Angle = 0;
            Coordinates = new float[] { x, y, Constants.DefaultZ };
            RotationMatrix = Matrix4.Identity;
        }
    }
}
