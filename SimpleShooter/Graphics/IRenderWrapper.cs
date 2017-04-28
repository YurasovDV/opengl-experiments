using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace SimpleShooter.Graphics
{
    interface IRenderWrapper
    {
        PrimitiveType RenderType { get; set; }
        int VerticesCount { get; }

        void Bind(Camera camera, Vector3 lightPosition);
    }
}
