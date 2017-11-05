using System;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using SimpleShooter.Core;
using SimpleShooter.Graphics.ShaderLoad;

namespace SimpleShooter.Graphics
{
    public class RenderWrapper : IRenderWrapper
    {
        private GameObject _gameObject;

        private ShaderProgramDescriptor _descriptor;

        public RenderWrapper(GameObject gameObject)
        {
            _gameObject = gameObject;
            bool loaded = ShaderLoader.TryGet(_gameObject.ShaderKind, out _descriptor);
            if (!loaded)
            {
                throw new InvalidOperationException();
            }
            if (_gameObject.ShaderKind == ShadersNeeded.Line)
            {
                RenderType = PrimitiveType.Lines;
            }
        }

        public int VerticesCount
        {
            get
            {
                return (_gameObject as GameObject).Model.Vertices.Length;
            }
        }

        public ShadersNeeded ShaderKind { get { return _gameObject.ShaderKind; } }

        private PrimitiveType _renderType = PrimitiveType.Triangles;

        public PrimitiveType RenderType
        {
            get { return _renderType; }
            set { _renderType = value; }
        }

        public void Bind(Camera camera, Level level)
        {
            GL.UseProgram(_descriptor.ProgramId);
            BindUniforms(_descriptor, camera, level.LightPosition, _gameObject.Model.TextureId);
            BindBuffers(_gameObject.ShaderKind);
        }

        public static void BindVertices(ShaderProgramDescriptor descriptor, Vector3[] data)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, descriptor.verticesBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(data.Length * Vector3.SizeInBytes),
                data, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(descriptor.AttribVerticesLocation, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(descriptor.AttribVerticesLocation);
        }

        public static void BindColors(ShaderProgramDescriptor descriptor, Vector3[] data)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, descriptor.colorsBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(data.Length * Vector3.SizeInBytes),
                data, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(descriptor.AttribColorsLocation, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(descriptor.AttribColorsLocation);
        }

        public static void BindNormals(ShaderProgramDescriptor descriptor, Vector3[] data)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, descriptor.normalsBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(data.Length * Vector3.SizeInBytes),
                data, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(descriptor.AttribNormalsLocation, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(descriptor.AttribNormalsLocation);
        }


        public static void BindTextureCoords(ShaderProgramDescriptor descriptor, Vector2[] data)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, descriptor.textureCoordsBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(data.Length * Vector2.SizeInBytes),
                data, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(descriptor.AttribTextureCoordsLocation, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(descriptor.AttribTextureCoordsLocation);
        }

        public ShaderProgramDescriptor GetDescriptor()
        {
            return _descriptor;
        }

        public static void BindUniforms(ShaderProgramDescriptor descriptor, Camera camera, Vector3 lightPos, int textureId = -1)
        {
            GL.UniformMatrix4(descriptor.uniformMV, false, ref camera.ModelView);
            GL.UniformMatrix4(descriptor.uniformMVP, false, ref camera.ModelViewProjection);
            GL.UniformMatrix4(descriptor.uniformProjection, false, ref camera.Projection);

            switch (descriptor.ShaderKind)
            {
                case ShadersNeeded.SimpleModel:
                    GL.Uniform3(descriptor.uniformLightPos, lightPos);
                    if (textureId != -1)
                    {
                        GL.ActiveTexture(TextureUnit.Texture0);
                        GL.Uniform1(descriptor.TextureSampler, 0);
                        GL.BindTexture(TextureTarget.Texture2D, textureId);
                    }

                    break;
                case ShadersNeeded.Line:
                    break;
                case ShadersNeeded.TextureLess:
                    GL.Uniform3(descriptor.uniformLightPos, lightPos);

                    break;

                case ShadersNeeded.TextureLessNoLight:

                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(descriptor.ShaderKind), descriptor.ShaderKind, null);
            }
        }

        private void BindBuffers(ShadersNeeded gameObjectShaderKind)
        {
            BindVertices();
            BindColors();

            switch (gameObjectShaderKind)
            {
                case ShadersNeeded.SimpleModel:
                    BindNormals();
                    BindTextureCoords();
                    break;
                case ShadersNeeded.TextureLess:
                    BindNormals();
                    break;
                case ShadersNeeded.Line:
                    break;
                case ShadersNeeded.TextureLessNoLight:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(gameObjectShaderKind), gameObjectShaderKind, null);
            }
        }

        private void BindVertices()
        {
            BindVertices(_descriptor, _gameObject.Model.Vertices);
        }

        private void BindColors()
        {
            BindColors(_descriptor, _gameObject.Model.Colors);
        }

        private void BindNormals()
        {
            BindNormals(_descriptor, _gameObject.Model.Normals);
        }

        private void BindTextureCoords()
        {
            BindTextureCoords(_descriptor, _gameObject.Model.TextureCoordinates);
        }
    }
}