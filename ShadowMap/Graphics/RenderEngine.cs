using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace ShadowMap
{
    public class RenderEngine : AbstractRenderEngine
    {
        public PrimitiveType RenderMode = PrimitiveType.Triangles;

        public int depthMapBuffer;

        public int depthMapBufferTextureId;

        public int renderBuffer;

        public ShaderManager Shaders { get; set; }

        public RenderEngine(int width, int height, IPlayer player, float zFar = 300)
            : base(width, height, player, zFar)
        {
            Shaders = new ShaderManager(this);

            var frameBuf = new TextureManager().GetFrameBuffer(width, height);
            depthMapBuffer = frameBuf.FramBufferObject;
            depthMapBufferTextureId = frameBuf.TextureId;
            renderBuffer = frameBuf.RenderBufferObject;
        }

        public void EnableFrameBuffer()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, depthMapBuffer);
            GL.Viewport(0, 0, Width, Height);
        }

        public void FlushFrameBuffer()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.Disable(EnableCap.DepthTest);
            GL.ClearColor(1, 0f, 0f, 0);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.UseProgram(Shaders.FrameBufferProgramId);

            GL.BindBuffer(BufferTarget.ArrayBuffer, Shaders.vertexBufferForFrameAddress);
            var points = GetFrameBufferVertices();
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(6 * Vector2.SizeInBytes), points, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(Shaders.AttrVertexFrameLocation, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(Shaders.AttrVertexFrameLocation);

            GL.BindBuffer(BufferTarget.ArrayBuffer, Shaders.texcoordsForFrameAddress);
            var texCoords = GetFrameBufferTextureCoords();
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(6 * Vector2.SizeInBytes), texCoords, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(Shaders.AttrTexcoordFrameLocation, 2, VertexAttribPointerType.Float, false, 0, 0);

            GL.EnableVertexAttribArray(Shaders.AttrTexcoordFrameLocation);


            GL.ActiveTexture(TextureUnit.Texture0);
            GL.Uniform1(Shaders.uniformTextureFrame, 0);
            GL.BindTexture(TextureTarget.Texture2D, depthMapBufferTextureId);
            
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
            GL.BindVertexArray(0);

            GL.Flush();
        }

        private Vector2[] GetFrameBufferVertices()
        {
            return quadVertices;
        }

        private Vector2[] GetFrameBufferTextureCoords()
        {
            return textQuadIndices;
        }

        public new void PreRender()
        {
            GL.ClearColor(0, 0f, 0f, 100);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);
            SetupVieport();
        }

        public void Draw(SimpleModel model, Vector3 light)
        {
            Shaders.BindBuffers(model, light);
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

        private readonly Vector2[] textQuadIndices = new[] {
              new Vector2(0.0f, 1.0f),
              new Vector2(0.0f, 0.0f),
              new Vector2(1.0f, 0.0f),

              new Vector2(0.0f, 1.0f),
              new Vector2(1.0f, 0.0f),
              new Vector2(1.0f, 1.0f)
        };

        private readonly Vector2[] quadVertices = new[] {
        // Positions  
        new Vector2(-1.0f,  1.0f),
        new Vector2(-1.0f, -1.0f),
        new Vector2(1.0f, -1.0f),

        new Vector2(-1.0f,  1.0f),
        new Vector2( 1.0f, -1.0f),
        new Vector2(1.0f,  1.0f),
        };
    }
}
