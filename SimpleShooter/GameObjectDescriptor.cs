using OpenTK.Graphics.OpenGL4;
using SimpleShooter.Core;
using SimpleShooter.Graphics;

namespace SimpleShooter
{
    public class GameObjectDescriptor : IRenderWrapper
    {
        public GameObjectDescriptor(GameObject o)
        {
            GameIdentity = o;
            RenderIdentity = new RenderWrapper(o);
        }

        public GameObject GameIdentity { get; set; }

        public IRenderWrapper RenderIdentity { get; set; }

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

        public ShadersNeeded ShaderKind
        {
            get
            {
                return RenderIdentity.ShaderKind;
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

        public ShaderProgramDescriptor GetDescriptor()
        {
            return RenderIdentity.GetDescriptor();
        }
    }
}
