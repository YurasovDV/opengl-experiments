using System.Collections.Generic;
using OpenTK;

namespace FabricSimulation
{
    public class PointMass
    {
        public const float gravity = 10000f;

        public Vector3 Location { get; set; }
        public Vector3 PreviousLocation { get; set; }

        public Vector3 PinnedLocation { get; set; }

        public bool IsFixedPosition { get; set; }

        public Vector3 Acceleration { get; set; }

        public float Mass { get; internal set; }

        public List<Constraint> Constraints { get; set; }

        public PointMass(Vector3 point, bool isPinned, float mass = 0.01f)
        {
            IsFixedPosition = isPinned;
            Constraints = new List<Constraint>();

            Location = new Vector3(point);
            PinnedLocation = new Vector3(point);
            PreviousLocation = new Vector3(point);

            Mass = mass;
        }

        public void OnInput()
        {
            /*

            float mouseWEight = 5;
            if (mousePressed)
            {
                float distanceSquared = distPointToSegmentSquared(pmouseX, pmouseY, mouseX, mouseY, x, y);
                if (mouseButton == LEFT)
                {
                    if (distanceSquared < mouseInfluenceSize)
                    { // remember mouseInfluenceSize was squared in setup()
                      // To change the velocity of our PointMass, we subtract that change from the lastPosition.
                      // When the physics gets integrated (see updatePhysics()), the change is calculated
                      // Here, the velocity is set equal to the cursor's velocity

                        PreviousLocation = new Vector3(Location - )

                        lastX = x - (mouseX - pmouseX)  * mouseWEight;
                        lastY = y - (mouseY - pmouseY);
                    }
                }

                else
                { // if the right mouse button is clicking, we tear the cloth by removing links
                    if (distanceSquared < mouseTearSize)
                        links.clear();
                }
            }*/
        }

        public void UpdatePhysics(float timeStep)
        {
            var g = new Vector3(0, -Mass * gravity, 0) / (1000 / timeStep);

            AddForce(g);

            // лол, трение
            var velocity = (Location - PreviousLocation) * 0.97f;
            float timeStepSq = timeStep * timeStep / 1e6f;

            var nextLocation = Location + velocity + Acceleration * 0.5f * timeStepSq;

            // reset variables
            PreviousLocation = Location;

            Location = nextLocation;

            Acceleration = new Vector3(0, 0, 0);
        }


        public void AddForce(Vector3 force)
        {
            Acceleration += force / Mass;
        }

        public void SolveConstraints()
        {
            foreach (var constraint in Constraints)
            {
                constraint.Solve();
            }

            if (IsFixedPosition)
            {
                Location = PinnedLocation;
            }
        }
    }
}
