using Common;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace ShadowMap
{
    public class RenderEngine : AbstractRenderEngine
    {
        public PrimitiveType RenderMode = PrimitiveType.Triangles;

        public bool FormShadowMap { get; set; }

        public ShaderManager Shaders { get; set; }

        public RenderEngine(int width, int height, IPlayer player, float zFar = 300)
            : base(width, height, player, zFar)
        {
            Shaders = new ShaderManager(this);
        }

        public override void SetupVieport()
        {
            if (FormShadowMap)
            {
                GL.Enable(EnableCap.DepthTest);
                var target = new Vector3(50, 0, 50);
                ModelView = Matrix4.LookAt(Player.FlashlightPosition, target, Vector3.UnitY);
                Projection = Matrix4.CreateOrthographic(Width, Height, 0, 100);

                ModelViewProjection = Matrix4.Mult(ModelView, Projection);
            }
            else
            {
                base.SetupVieport();
            }
        }

        public new void PreRender()
        {
            GL.ClearColor(0, 0.5f, 0.75f, 100);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);
            SetupVieport();
        }

        public void Draw(SimpleModel model, Vector3 light, Matrix4 lightSpaceMVP, int? depthMapBuf = null)
        {
            Shaders.BindBuffers(model, light, lightSpaceMVP, depthMapBuf);
            GL.DrawArrays(RenderMode, 0, model.Vertices.Length);
            Shaders.Release();
        }

        public void Draw(SimpleModel model, Vector3 light, int? depthMapBuf = null)
        {
            Matrix4 lightSpaceMVP = Matrix4.Identity;
            Shaders.BindBuffers(model, light, lightSpaceMVP, depthMapBuf);
            GL.DrawArrays(RenderMode, 0, model.Vertices.Length);
            Shaders.Release();
        }

        public new void PostRender()
        {

        }

        internal void ChangeRenderMode()
        {
            if (RenderMode == PrimitiveType.Triangles)
            {
                RenderMode = PrimitiveType.Lines;
            }
            else
            {
                RenderMode = PrimitiveType.Triangles;
            }
        }
    }
}
