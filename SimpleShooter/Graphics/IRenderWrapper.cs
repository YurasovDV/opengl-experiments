using OpenTK.Graphics.OpenGL4;
using SimpleShooter.Core;

namespace SimpleShooter.Graphics
{
    interface IRenderWrapper
    {
        ShadersNeeded ShaderKind { get; }

        PrimitiveType RenderType { get; set; }
        int VerticesCount { get; }

        void Bind(Camera camera, Level level);
    }
}
