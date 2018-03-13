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


        public static void BindGBufferTextures(FrameBufferDesc bufferHandle)
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.Uniform1(_fullScreenQuadProgram.uniformTextureColor, 0);
            GL.BindTexture(TextureTarget.Texture2D, bufferHandle.ColorAndSpectacularTextureId);
        }

        private static void BindLightingTextures(FrameBufferDesc lightingFBO)
        {
            GL.ActiveTexture(TextureUnit.Texture3);
            GL.Uniform1(_fullScreenQuadProgram.uniformTextureDiffuse, 3);
            GL.BindTexture(TextureTarget.Texture2D, lightingFBO.DiffuseTextureId);
        }

        public static void PrepareToDrawLights(FrameBufferManager frameBufferManager, Matrix4 modelView, Matrix4 modelViewProjection)
        {
            var descriptor = _lightDescriptor;

            GL.UseProgram(descriptor.ProgramId);

            GL.UniformMatrix4(descriptor.uniformMV, false, ref modelView);
            GL.UniformMatrix4(descriptor.uniformMVP, false, ref modelViewProjection);

            FrameBufferDesc bufferHandle = frameBufferManager.GeometryFrameBufferDescriptorDescriptor;


            GL.ActiveTexture(TextureUnit.Texture0);
            GL.Uniform1(descriptor.uniformTexturePos, 0);
            GL.BindTexture(TextureTarget.Texture2D, bufferHandle.PositionTextureId);

            GL.ActiveTexture(TextureUnit.Texture1);
            GL.Uniform1(descriptor.uniformTextureNormal, 1);
            GL.BindTexture(TextureTarget.Texture2D, bufferHandle.NormalTextureId);

            GL.ActiveTexture(TextureUnit.Texture2);
            GL.Uniform1(descriptor.uniformTextureColor, 2);
            GL.BindTexture(TextureTarget.Texture2D, bufferHandle.ColorAndSpectacularTextureId);
        }

        public static void DrawLight(PointLight light)
        {
            var descriptor = _lightDescriptor;

            GL.BindBuffer(BufferTarget.ArrayBuffer, descriptor.verticesBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(light.Vertices.Length * Vector3.SizeInBytes),
                light.Vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(descriptor.AttribVerticesLocation, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(descriptor.AttribVerticesLocation);

            GL.Uniform3(descriptor.uniformColor, light.Color);
            GL.Uniform3(descriptor.uniformPosition, light.Center);

            GL.DrawArrays(PrimitiveType.Triangles, 0, light.Vertices.Length);
        }


        public static void BindOneQuadScreenAndDraw(FrameBufferManager frameBufferManager, Vector3 playerPos)
        {
            GL.UseProgram(_fullScreenQuadProgram.ProgramId);

            var points = frameBufferManager.GetFrameBufferVertices();
            var texCoords = frameBufferManager.GetFrameBufferTextureCoords();

            BindGBufferPart(points, texCoords);

            FrameBufferDesc bufferHandle = frameBufferManager.GeometryFrameBufferDescriptorDescriptor;
            BindGBufferTextures(bufferHandle);
            BindLightingTextures(frameBufferManager.LightingFrameBufferDescriptorDescriptor);

             GL.DrawArrays(PrimitiveType.Triangles, 0, 6);

            GL.Disable(EnableCap.Blend);

            DrawAuxillaryBuffers(frameBufferManager, bufferHandle, frameBufferManager.LightingFrameBufferDescriptorDescriptor, texCoords);
        }

        private static void DrawAuxillaryBuffers(FrameBufferManager frameBufferManager, FrameBufferDesc bufferHandle, FrameBufferDesc lightingbufferHandle,
            Vector2[] texCoords)
        {
            GL.UseProgram(_auxillaryProgram.ProgramId);
            GL.Uniform1(_auxillaryProgram.IsDepth, 0);
            var points = frameBufferManager.GetFrameBufferVertices(FramebufferAttachment.ColorAttachment0);
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
            GL.BindTexture(TextureTarget.Texture2D, lightingbufferHandle.DiffuseTextureId);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);


            points = frameBufferManager.GetFrameBufferVertices(FramebufferAttachment.DepthAttachment);
            // texCoords = frameBufferManager.GetFrameBufferTextureCoords();
            BindGBufferPart(points, texCoords);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.Uniform1(_auxillaryProgram.uniformTexture0, 0);
            GL.Uniform1(_auxillaryProgram.IsDepth, 1);
            GL.BindTexture(TextureTarget.Texture2D, bufferHandle.DepthTextureId);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
        }

        private static void BindGBufferPart(Vector2[] points, Vector2[] texCoords)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, _fullScreenQuadProgram.verticesBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(6 * Vector2.SizeInBytes), points, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(_fullScreenQuadProgram.AttribVerticesLocation, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(_fullScreenQuadProgram.AttribVerticesLocation);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _fullScreenQuadProgram.texCoordsBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(6 * Vector2.SizeInBytes), texCoords, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(_fullScreenQuadProgram.TexCoordsLocation, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(_fullScreenQuadProgram.TexCoordsLocation);
        }
    }
}
