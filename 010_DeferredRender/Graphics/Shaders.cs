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


            GL.BindBuffer(BufferTarget.ArrayBuffer, descriptor.normalsBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(model.Normals.Length * Vector3.SizeInBytes),
                model.Colors, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(descriptor.AttribNormalsLocation, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(descriptor.AttribNormalsLocation);
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


            GL.BindBuffer(BufferTarget.ArrayBuffer, descriptor.normalsBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(model.Normals.Length * Vector3.SizeInBytes),
                model.Normals, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(descriptor.AttribNormalsLocation, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(descriptor.AttribNormalsLocation);


            GL.BindBuffer(BufferTarget.ArrayBuffer, descriptor.texCoordsBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(model.TextureCoordinates.Length * Vector2.SizeInBytes),
              model.TextureCoordinates, BufferUsageHint.DynamicDraw);
            GL.VertexAttribPointer(descriptor.TexCoordsLocation, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(descriptor.TexCoordsLocation);

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.Uniform1(descriptor.uniformTexture1, 0);
            GL.BindTexture(TextureTarget.Texture2D, model.TextureId);
        }


        public static void BindOneQuadScreenAndDraw(FrameBufferManager frameBufferManager, Vector3 playerPos, List<SimpleModel> lights)
        {
            FrameBufferDesc bufferHandle = frameBufferManager.GBufferDescriptor;
            GL.UseProgram(_secondGBufferPassDescriptor.ProgramId);

            var points = frameBufferManager.GetFrameBufferVertices();
            var texCoords = frameBufferManager.GetFrameBufferTextureCoords();

            BindGBufferPart(points, texCoords);

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.Uniform1(_secondGBufferPassDescriptor.uniformTexturePos, 0);
            GL.BindTexture(TextureTarget.Texture2D, bufferHandle.PositionTextureId);

            GL.ActiveTexture(TextureUnit.Texture1);
            GL.Uniform1(_secondGBufferPassDescriptor.uniformTextureNormal, 1);
            GL.BindTexture(TextureTarget.Texture2D, bufferHandle.NormalTextureId);

            GL.ActiveTexture(TextureUnit.Texture2);
            GL.Uniform1(_secondGBufferPassDescriptor.uniformTextureColor, 2);
            GL.BindTexture(TextureTarget.Texture2D, bufferHandle.ColorAndSpectacularTextureId);

            GL.Uniform3(_secondGBufferPassDescriptor.UniformCameraPosition, playerPos);

            for (int i = 0; i < lights.Count; i++)
            {
                var location = GL.GetUniformLocation(_secondGBufferPassDescriptor.ProgramId, "lights[" + i + "].pos");
                GL.Uniform3(location, lights[i].Vertices[0]);

                location = GL.GetUniformLocation(_secondGBufferPassDescriptor.ProgramId, "lights[" + i + "].color");
                GL.Uniform3(location, lights[i].Colors[0]);

                location = GL.GetUniformLocation(_secondGBufferPassDescriptor.ProgramId, "lights[" + i + "].radius");
                GL.Uniform1(location, lights[i].Normals[0].X);
            }

            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);

            GL.UseProgram(_auxillaryProgram.ProgramId);

            points = frameBufferManager.GetFrameBufferVertices(FramebufferAttachment.ColorAttachment0);
            BindGBufferPart(points, texCoords);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.Uniform1(_auxillaryProgram.uniformTexture0, 0);
            GL.BindTexture(TextureTarget.Texture2D, bufferHandle.PositionTextureId);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);


            points = frameBufferManager.GetFrameBufferVertices(FramebufferAttachment.ColorAttachment1);
            // texCoords = frameBufferManager.GetFrameBufferTextureCoords();
            BindGBufferPart(points, texCoords);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.Uniform1(_auxillaryProgram.uniformTexture0, 0);
            GL.BindTexture(TextureTarget.Texture2D, bufferHandle.NormalTextureId);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);


            points = frameBufferManager.GetFrameBufferVertices(FramebufferAttachment.ColorAttachment2);
            // texCoords = frameBufferManager.GetFrameBufferTextureCoords();
            BindGBufferPart(points, texCoords);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.Uniform1(_auxillaryProgram.uniformTexture0, 0);
            GL.BindTexture(TextureTarget.Texture2D, bufferHandle.ColorAndSpectacularTextureId);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);


            points = frameBufferManager.GetFrameBufferVertices(FramebufferAttachment.DepthAttachment);
            // texCoords = frameBufferManager.GetFrameBufferTextureCoords();
            BindGBufferPart(points, texCoords);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.Uniform1(_auxillaryProgram.uniformTexture0, 0);
            GL.BindTexture(TextureTarget.Texture2D, bufferHandle.DepthTextureId);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);


        }

        private static void BindGBufferPart(Vector2[] points, Vector2[] texCoords)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, _secondGBufferPassDescriptor.verticesBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(6 * Vector2.SizeInBytes), points, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(_secondGBufferPassDescriptor.AttribVerticesLocation, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(_secondGBufferPassDescriptor.AttribVerticesLocation);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _secondGBufferPassDescriptor.texCoordsBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(6 * Vector2.SizeInBytes), texCoords, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(_secondGBufferPassDescriptor.TexCoordsLocation, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(_secondGBufferPassDescriptor.TexCoordsLocation);
        }
    }
}
