using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using OpenTK;

namespace DeferredRender
{
    class PointLight : SimpleModel
    {
        public Vector3 Center { get; set; }

        public float Radius { get; set; }

        public Vector3 Color
        {
            get
            {
                if (Colors == null)
                {
                    return Vector3.Zero;
                }
                return Colors.FirstOrDefault();
            }
        }

        public Matrix4 Transform;
    }
}
