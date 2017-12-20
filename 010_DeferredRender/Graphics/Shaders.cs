using System;
using System.Collections.Generic;
using System.IO;
using Common;
using DeferredRender.Graphics.FrameBuffer;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace DeferredRender.Graphics
{
    partial class Shaders
    {
       public static void BindTexturelessNoLight(SimpleModel model, Matrix4 modelView, Matrix4 modelViewProjection, Matrix4 projection)
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


        public static void BindTexturedNoLight(SimpleModel model, Matrix4 modelView, Matrix4 modelViewProjection, Matrix4 projection)
        {
            var descriptor = _texturedNoLightDescriptor;

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


            GL.BindBuffer(BufferTarget.ArrayBuffer, descriptor.texCoordsBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(model.TextureCoordinates.Length * Vector2.SizeInBytes),
              model.TextureCoordinates, BufferUsageHint.DynamicDraw);
            GL.VertexAttribPointer(descriptor.TexCoordsLocation, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(descriptor.TexCoordsLocation);

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.Uniform1(descriptor.uniformTexture1, 0);
            GL.BindTexture(TextureTarget.Texture2D, model.TextureId);
        }

        public static void BindOneQuadScreen(FrameBufferManager frameBufferManager)
        {
            var FrameBufferProgramId = _secondGBufferPassDescriptor.ProgramId;
            FrameBufferDesc bufferHandle = frameBufferManager.GBuferDescriptor;
            GL.UseProgram(FrameBufferProgramId);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _secondGBufferPassDescriptor.verticesBuffer);
            var points = frameBufferManager.GetFrameBufferVertices();
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(6 * Vector2.SizeInBytes), points, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(_secondGBufferPassDescriptor.AttribVerticesLocation, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(_secondGBufferPassDescriptor.AttribVerticesLocation);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _secondGBufferPassDescriptor.texCoordsBuffer);
            var texCoords = frameBufferManager.GetFrameBufferTextureCoords();
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(6 * Vector2.SizeInBytes), texCoords, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(_secondGBufferPassDescriptor.TexCoordsLocation, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(_secondGBufferPassDescriptor.TexCoordsLocation);


            GL.ActiveTexture(TextureUnit.Texture0);
            GL.Uniform1(_secondGBufferPassDescriptor.uniformTexture1, 0);
            GL.BindTexture(TextureTarget.Texture2D, bufferHandle.PositionTextureId);



            /*
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.Uniform1(descriptor.uniformTexture1, 0);
            GL.BindTexture(TextureTarget.Texture2D, model.TextureId);*/




        }
    }
}
