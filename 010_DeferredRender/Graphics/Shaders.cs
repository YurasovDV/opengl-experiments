using System;
using System.Collections.Generic;
using System.IO;
using Common;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace DeferredRender.Graphics
{
    class Shaders
    {
        private static TexturelessNoLight _texturelessNoLightDescriptor;

        class TexturelessNoLight
        {
            public int uniformMVP = 0;
            public int uniformMV = 0;
            public int uniformProjection = 0;
            public int ProgramId = 0;
            public int AttribVerticesLocation = 0;
            public int AttribNormalsLocation = 0;
            public int AttribColorsLocation = 0;
            public int verticesBuffer = 0;
            public int colorsBuffer = 0;
            public int normalsBuffer = 0;
        }

        public static void InitTexturelessNoLight()
        {
           
            var textureLessProgId = GL.CreateProgram();

            var vert = GL.CreateShader(ShaderType.VertexShader);
            var vertText = File.ReadAllText(@"Assets\Shaders\texturelessNoLight.vert");
            GL.ShaderSource(vert, vertText);
            GL.CompileShader(vert);
            GL.AttachShader(textureLessProgId, vert);


            int statusCode;
            GL.GetShader(vert, ShaderParameter.CompileStatus, out statusCode);
            if (statusCode != 1)
            {
                string info;
                GL.GetShaderInfoLog(vert, out info);
                throw new Exception("vertex shader: " + info);
            }

            var frag = GL.CreateShader(ShaderType.FragmentShader);
            var fragText = File.ReadAllText(@"Assets\Shaders\texturelessNoLight.frag");
            GL.ShaderSource(frag, fragText);
            GL.CompileShader(frag);
            GL.AttachShader(textureLessProgId, frag);

            GL.LinkProgram(textureLessProgId);

            GL.GetShader(frag, ShaderParameter.CompileStatus, out statusCode);
            if (statusCode != 1)
            {
                string info;
                GL.GetShaderInfoLog(frag, out info);
                throw new Exception("fragment shader: " + info);
            }

            _texturelessNoLightDescriptor = new TexturelessNoLight();

            _texturelessNoLightDescriptor.uniformMVP = GL.GetUniformLocation(textureLessProgId, "uMVP");
            _texturelessNoLightDescriptor.uniformMV = GL.GetUniformLocation(textureLessProgId, "uMV");
            _texturelessNoLightDescriptor.uniformProjection = GL.GetUniformLocation(textureLessProgId, "uP");

            _texturelessNoLightDescriptor.ProgramId = textureLessProgId;

            _texturelessNoLightDescriptor.AttribVerticesLocation = GL.GetAttribLocation(textureLessProgId, "vPosition");
            _texturelessNoLightDescriptor.AttribNormalsLocation = GL.GetAttribLocation(textureLessProgId, "vNormal");
            _texturelessNoLightDescriptor.AttribColorsLocation = GL.GetAttribLocation(textureLessProgId, "vColor");

            GL.GenBuffers(1, out _texturelessNoLightDescriptor.verticesBuffer);
            GL.GenBuffers(1, out _texturelessNoLightDescriptor.colorsBuffer);
            GL.GenBuffers(1, out _texturelessNoLightDescriptor.normalsBuffer);


        }

        internal static void BindTexturelessNoLight(SimpleModel model, Matrix4 modelView, Matrix4 modelViewProjection, Matrix4 projection)
        {
            var descriptor = _texturelessNoLightDescriptor;

            GL.UseProgram(descriptor.ProgramId);

            GL.UniformMatrix4(descriptor.uniformMV, false, ref modelView);
            GL.UniformMatrix4(descriptor.uniformMVP, false, ref modelViewProjection);
            GL.UniformMatrix4(descriptor.uniformProjection, false, ref projection);


            GL.BindBuffer(BufferTarget.ArrayBuffer, descriptor.verticesBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(model.Vertices.Length * Vector3.SizeInBytes),
                model.Vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(descriptor.AttribVerticesLocation, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(descriptor.AttribVerticesLocation);


            GL.BindBuffer(BufferTarget.ArrayBuffer, descriptor.colorsBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(model.Colors.Length * Vector3.SizeInBytes),
                model.Colors, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(descriptor.AttribColorsLocation, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(descriptor.AttribColorsLocation);


        }
    }
}
