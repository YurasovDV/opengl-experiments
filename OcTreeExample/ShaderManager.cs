using Common;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OcTreeExample
{
    class ShaderManager : AbstractShaderManager
    {

        public int VertexAttribLocation { get; set; }
        public int ColorAttribLocation { get; set; }
        public int NormalAttribLocation { get; set; }

        public int AttributeTexcoord_Parameter_Address { get; set; }

        public int Uniform_ModelViewProjection { get; set; }
        public int Uniform_Projection { get; set; }
        public int Uniform_ModelView { get; set; }

        public int Uniform_LightPos { get; set; }

        public int UniformTexture_Parameter_Address { get; set; }

        public int UniformViewSkybox { get; set; }
        public int UniformProjectionSkybox { get; set; }
        public int AttribPositionSkybox { get; set; }
        public int UniformSkySampler { get; set; }

        public AbstractRenderEngine renderEngine { get; set; }


        public ShaderManager(RenderEngine renderEngine)
            : base()
        {
            this.renderEngine = renderEngine;
            CreateMainProgram(@"Assets\Shaders\vert.glsl", @"Assets\Shaders\frag.glsl");

            VertexAttribLocation = GL.GetAttribLocation(ProgramId, "vPosition");
            ColorAttribLocation = GL.GetAttribLocation(ProgramId, "vColor");
            NormalAttribLocation = GL.GetAttribLocation(ProgramId, "vNormal");

            Uniform_ModelViewProjection = GL.GetUniformLocation(ProgramId, "u_modelViewprojection");
            Uniform_Projection = GL.GetUniformLocation(ProgramId, "u_projection");
            Uniform_ModelView = GL.GetUniformLocation(ProgramId, "u_modelview");

            Uniform_LightPos = GL.GetUniformLocation(ProgramId, "vLightPosition");

            AttributeTexcoord_Parameter_Address = GL.GetAttribLocation(ProgramId, "a_TexCoordinate");
            UniformTexture_Parameter_Address = GL.GetUniformLocation(ProgramId, "u_Texture");

        }

        public override void BindBuffers(SimpleModel model, Vector3 light, RefreshKind refresh)
        {
            GL.UseProgram(ProgramId);

            GL.UniformMatrix4(Uniform_ModelViewProjection, false, ref renderEngine.ModelViewProjection);
            GL.UniformMatrix4(Uniform_ModelView, false, ref renderEngine.ModelViewProjection);
            GL.UniformMatrix4(Uniform_Projection, false, ref renderEngine.Projection);

            GL.Uniform3(Uniform_LightPos, ref light);



            if (firstDraw || refresh != RefreshKind.None)
            {
                if (firstDraw || refresh.HasFlag(RefreshKind.Vertices))
                {
                    GL.BindBuffer(BufferTarget.ArrayBuffer, vertex_buffer_address);
                    GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(model.Vertices.Length * Vector3.SizeInBytes),
                        model.Vertices, BufferUsageHint.StaticDraw);
                    GL.VertexAttribPointer(VertexAttribLocation, 3, VertexAttribPointerType.Float, false, 0, 0);
                }


                if (firstDraw || refresh.HasFlag(RefreshKind.Color))
                {
                    GL.BindBuffer(BufferTarget.ArrayBuffer, color_buffer_address);
                    GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(model.Colors.Length * Vector3.SizeInBytes),
                        model.Colors, BufferUsageHint.StaticDraw);
                    GL.VertexAttribPointer(ColorAttribLocation, 3, VertexAttribPointerType.Float, false, 0, 0);
                }

                if (firstDraw || refresh.HasFlag(RefreshKind.TextureCoords))
                {


                    GL.BindBuffer(BufferTarget.ArrayBuffer, texcoord_buffer_address);
                    GL.BufferData<Vector2>(BufferTarget.ArrayBuffer, (IntPtr)(model.TextureCoordinates.Length * Vector2.SizeInBytes),
                           model.TextureCoordinates, BufferUsageHint.StaticDraw);
                    GL.VertexAttribPointer(AttributeTexcoord_Parameter_Address, 2, VertexAttribPointerType.Float, false, 0, 0);

                }
                firstDraw = false;
            }

            /*GL.BindBuffer(BufferTarget.ArrayBuffer, normal_buffer_address);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(model.Normals.Length * Vector3.SizeInBytes),
                model.Normals, BufferUsageHint.DynamicDraw);
            GL.VertexAttribPointer(NormalAttribLocation, 3, VertexAttribPointerType.Float, false, 0, 0);*/

            GL.EnableVertexAttribArray(VertexAttribLocation);
            GL.EnableVertexAttribArray(ColorAttribLocation);
            // GL.EnableVertexAttribArray(NormalAttribLocation);
            GL.EnableVertexAttribArray(AttributeTexcoord_Parameter_Address);
        }


        public override void BindBuffers(SimpleModel model, Vector3 light, bool refreshVertices = false, bool refreshColors = false)
        {
            GL.UseProgram(ProgramId);

            GL.UniformMatrix4(Uniform_ModelViewProjection, false, ref renderEngine.ModelViewProjection);
            GL.UniformMatrix4(Uniform_ModelView, false, ref renderEngine.ModelViewProjection);
            GL.UniformMatrix4(Uniform_Projection, false, ref renderEngine.Projection);

            GL.Uniform3(Uniform_LightPos, ref light);



            if (firstDraw || refreshVertices || refreshColors)
            {
                if (firstDraw || refreshVertices)
                {
                    GL.BindBuffer(BufferTarget.ArrayBuffer, vertex_buffer_address);
                    GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(model.Vertices.Length * Vector3.SizeInBytes),
                        model.Vertices, BufferUsageHint.StaticDraw);
                    GL.VertexAttribPointer(VertexAttribLocation, 3, VertexAttribPointerType.Float, false, 0, 0);
                }


                if (firstDraw || refreshColors)
                {
                    GL.BindBuffer(BufferTarget.ArrayBuffer, color_buffer_address);
                    GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(model.Colors.Length * Vector3.SizeInBytes),
                        model.Colors, BufferUsageHint.StaticDraw);
                    GL.VertexAttribPointer(ColorAttribLocation, 3, VertexAttribPointerType.Float, false, 0, 0);
                }


                GL.BindBuffer(BufferTarget.ArrayBuffer, texcoord_buffer_address);
                GL.BufferData<Vector2>(BufferTarget.ArrayBuffer, (IntPtr)(model.TextureCoordinates.Length * Vector2.SizeInBytes),
                       model.TextureCoordinates, BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(AttributeTexcoord_Parameter_Address, 2, VertexAttribPointerType.Float, false, 0, 0);

                firstDraw = false;
            }

            /*GL.BindBuffer(BufferTarget.ArrayBuffer, normal_buffer_address);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(model.Normals.Length * Vector3.SizeInBytes),
                model.Normals, BufferUsageHint.DynamicDraw);
            GL.VertexAttribPointer(NormalAttribLocation, 3, VertexAttribPointerType.Float, false, 0, 0);*/

            GL.EnableVertexAttribArray(VertexAttribLocation);
            GL.EnableVertexAttribArray(ColorAttribLocation);
            // GL.EnableVertexAttribArray(NormalAttribLocation);
            GL.EnableVertexAttribArray(AttributeTexcoord_Parameter_Address);
        }
    }
}
