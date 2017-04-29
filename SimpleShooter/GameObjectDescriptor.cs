using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OcTreeLibrary;
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
            GameIdentity = o;
            RenderIdentity = new RenderWrapper(o);
            TreeIdentity = null;
        }

        public GameObject GameIdentity { get; set; }

        public IRenderWrapper RenderIdentity { get; set; }

        public IOctreeItem TreeIdentity { get; set; }

        public PrimitiveType RenderType
        {
            get
            {
                return RenderIdentity.RenderType;
            }

            set
            {
                RenderIdentity.RenderType = value;
            }
        }

        public int VerticesCount
        {
            get
            {
                return RenderIdentity.VerticesCount;
            }
        }

        public void Bind(Camera camera, Level level)
        {
            RenderIdentity.Bind(camera, level);
        }
    }
}
