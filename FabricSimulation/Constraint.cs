using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace FabricSimulation
{
    public class Constraint
    {
        public PointMass Mass1 { get; set; }
        public PointMass Mass2 { get; set; }
        public float NormalDistance { get; private set; }

        public Constraint(PointMass m1, PointMass m2, float normaldistance)
        {
            Mass1 = m1;
            Mass2 = m2;
            NormalDistance = normaldistance;
        }

        public void Solve()
        {
            var distance = (Mass1.Location - Mass2.Location);

            var distScalar = distance.Length;

            float tension = (NormalDistance - distScalar) / distScalar;

            var inversedMass1 = 1.0f / Mass1.Mass;
            var inversedMass2 = 1.0f / Mass2.Mass;

            var stiffness = 0.7f;
            float scalarP1 = (inversedMass1 / (inversedMass1 + inversedMass2)) * stiffness;
            float scalarP2 = stiffness - scalarP1;

            var firstPointShift = (distance * tension * scalarP1);
            var secondPointShift = (distance * tension * scalarP2);

            if (Mass1.IsFixedPosition && Mass2.IsFixedPosition)
            {

            }
            else if (!Mass1.IsFixedPosition && Mass2.IsFixedPosition)
            {
                Mass1.Location = Mass1.Location + firstPointShift + secondPointShift;
            }

            else if (Mass1.IsFixedPosition && !Mass2.IsFixedPosition)
            {
                Mass2.Location = Mass2.Location - secondPointShift - firstPointShift;
            }
            else if (!Mass1.IsFixedPosition && !Mass2.IsFixedPosition)
            {
                Mass1.Location = Mass1.Location + firstPointShift;
                Mass2.Location = Mass2.Location - secondPointShift;
            }
        }
    }
}
