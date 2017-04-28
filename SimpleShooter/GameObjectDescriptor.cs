using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using SimpleShooter.Core;
using SimpleShooter.Graphics;

namespace SimpleShooter
{
    class GameObjectDescriptor : IRenderWrapper
    {
        public GameObjectDescriptor(GameObject o)
        {
            PhysicalObject = o;
            RenderObject = new RenderWrapper(o);
        }

        public GameObject PhysicalObject { get; set; }

        public IRenderWrapper RenderObject { get; set; }

        public PrimitiveType RenderType
        {
            get
            {
                return RenderObject.RenderType;
            }

            set
            {
                RenderObject.RenderType = value;
            }
        }

        public int VerticesCount
        {
            get
            {
                return RenderObject.VerticesCount;
            }
        }

        public void Bind(Camera camera, Level level)
        {
            RenderObject.Bind(camera, level);
        }
    }
}
