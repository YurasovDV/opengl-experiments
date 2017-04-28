using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using SimpleShooter.Core;

namespace SimpleShooter.Graphics
{
    interface IRenderWrapper
    {
        PrimitiveType RenderType { get; set; }
        int VerticesCount { get; }

        void Bind(Camera camera, Level level);
    }
}
