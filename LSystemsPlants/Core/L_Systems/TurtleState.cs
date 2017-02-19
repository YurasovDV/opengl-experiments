using OpenTK;

namespace LSystemsPlants.Core.L_Systems
{
    public class TurtleState
    {
        public Matrix4 RotationMatrix;

        public float[] Coordinates { get; set; }

        public float Angle { get; set; }

        public TurtleState()
        {
            Angle = 0;
            Coordinates = new float[] { 0, 0, Constants.DefaultZ };
            RotationMatrix = Matrix4.Identity;
        }
    }
}
