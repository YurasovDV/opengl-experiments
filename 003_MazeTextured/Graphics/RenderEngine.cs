using System;
using Common;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using SimpleShadows.Core.Models;

namespace SimpleShadows.Graphics
{
    class RenderEngine
    {
        private ShaderManager ShaderManager { get; set; }

        public SkyboxRenderer Skybox { get; set; }

        public float Height { get; set; }

        public float Width { get; set; }

        public Matrix4 ModelView = Matrix4.Identity;
        public Matrix4 Projection = Matrix4.Identity;
        public Matrix4 ModelViewProjection = Matrix4.Identity;

        /// <summary>
        /// number of trinagles in one wall
        /// </summary>
        public const int TRIANGLES_IN_WALL = 30;

        public Player Player { get; set; }

        public RenderEngine(int Width, int Height, Player p)
        {
            this.Width = Width;
            this.Height = Height;
            Player = p;
            GL.Viewport(0, 0, (int)Width, (int)Height);
            float aspect = Width / Height;

            Projection = Matrix4.CreatePerspectiveFieldOfView(0.5f, aspect, 0.1f, 200);

            ShaderManager = new ShaderManager(this);

            Skybox = new SkyboxRenderer(ShaderManager, this);

        }

        internal void Render(SimpleModel model)
        {
            SetupVieport();

            GL.Disable(EnableCap.DepthTest);
            Skybox.Render();
            GL.Enable(EnableCap.DepthTest);

            BindBuffers(model);
            Draw(model);

            DrawLight();
            GL.Flush();
        }

        private void SetupVieport()
        {
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.ProgramPointSize);
            ModelView = Matrix4.LookAt(Player.Position, Player.Target, Vector3.UnitY);
            ModelViewProjection = Matrix4.Mult(ModelView, Projection);

            GL.ClearColor(0, 0f, 0.1f, 100);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        private void BindBuffers(SimpleModel model)
        {
            ShaderManager.BindBuffers(model, Player.FlashlightPosition, refreshVertices: true, refreshColors: true);
        }

        private void Draw(SimpleModel model)
        {
            if (model.TextureId != -1)
            {

                GL.BindBuffer(BufferTarget.ArrayBuffer, ShaderManager.texcoord_buffer_address);
                GL.BufferData<Vector2>(BufferTarget.ArrayBuffer, (IntPtr)(model.TextureCoordinates.Length * Vector2.SizeInBytes),
                  model.TextureCoordinates, BufferUsageHint.DynamicDraw);
                GL.VertexAttribPointer(ShaderManager.AttributeTexcoord_Parameter_Address, 2, VertexAttribPointerType.Float, false, 0, 0);

                GL.EnableVertexAttribArray(ShaderManager.AttributeTexcoord_Parameter_Address);

                GL.ActiveTexture(TextureUnit.Texture0);
                GL.Uniform1(ShaderManager.UniformTexture_Parameter_Address, 0);
                GL.BindTexture(TextureTarget.Texture2D, model.TextureId);

            }

            GL.DrawArrays(PrimitiveType.Triangles, 0, model.Vertices.Length);

            GL.BindTexture(TextureTarget.Texture2D, 0);
        }


        private void DrawLight()
        {
            GL.UseProgram(ShaderManager.ProgramIdForLight);
            int pointMVPMatrixHandle = GL.GetUniformLocation(ShaderManager.ProgramIdForLight, "u_MVPMatrix");
            int pointPositionHandle = GL.GetAttribLocation(ShaderManager.ProgramIdForLight, "a_Position");

            GL.DisableVertexAttribArray(pointPositionHandle);

            GL.VertexAttrib3(pointPositionHandle, Player.FlashlightPosition);
            GL.UniformMatrix4(pointMVPMatrixHandle, false, ref ModelViewProjection);

            GL.DrawArrays(PrimitiveType.Points, 0, 1);
        }
    }
}
