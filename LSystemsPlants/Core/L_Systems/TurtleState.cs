namespace LSystemsPlants.Core.L_Systems
{
    public class TurtleState
    {
        public float[] Coordinates { get; set; }

        public float Angle { get; set; }

        public TurtleState()
        {
            Angle = 0;
            Coordinates = new float[] { 0, 0, Constants.DefaultZ };
        }
    }
}
