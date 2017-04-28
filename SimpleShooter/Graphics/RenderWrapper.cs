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
            _descriptor = ShaderLoader.Load(_gameObject.ShaderKind);
        }

        public int VerticesCount
        {
            get
            {
                return (_gameObject as GameObject).Model.Vertices.Length;
            }
        }

        private PrimitiveType _renderType = PrimitiveType.Triangles;

        public PrimitiveType RenderType
        {
            get { return _renderType; }
            set { _renderType = value; }
        }

        public void Bind(Camera camera, Level level)
        {
            GL.UseProgram(_descriptor.ProgramId);
            BindUniforms(camera, level.LightPosition);
            BindBuffers(_gameObject.ShaderKind);
        }

        private void BindUniforms(Camera camera, Vector3 lightPos)
        {
            GL.UniformMatrix4(_descriptor.uniformMV, false, ref camera.ModelView);
            GL.UniformMatrix4(_descriptor.uniformMVP, false, ref camera.ModelViewProjection);
            GL.UniformMatrix4(_descriptor.uniformProjection, false, ref camera.Projection);

            switch (_gameObject.ShaderKind)
            {
                case ShadersNeeded.SimpleModel:
                    break;
                case ShadersNeeded.Line:
                    break;
                case ShadersNeeded.TextureLess:
                    GL.Uniform3(_descriptor.uniformLightPos, lightPos);

                    break;

                case ShadersNeeded.TextureLessNoLight:


                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_gameObject.ShaderKind), _gameObject.ShaderKind, null);
            }

        }

        private void BindBuffers(ShadersNeeded gameObjectShaderKind)
        {
            switch (gameObjectShaderKind)
            {
                case ShadersNeeded.SimpleModel:
                    break;
                case ShadersNeeded.Line:
                    break;
                case ShadersNeeded.TextureLess:
                    BindVertices();
                    BindColors();
                    BindNormals();

                    break;

                case ShadersNeeded.TextureLessNoLight:
                    BindVertices();
                    BindColors();

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(gameObjectShaderKind), gameObjectShaderKind, null);
            }
        }

        private void BindVertices()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, _descriptor.verticesBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr) (_gameObject.Model.Vertices.Length * Vector3.SizeInBytes),
                _gameObject.Model.Vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(_descriptor.AttribVerticesLocation, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(_descriptor.AttribVerticesLocation);
        }

        private void BindColors()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, _descriptor.colorsBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr) (_gameObject.Model.Colors.Length * Vector3.SizeInBytes),
                _gameObject.Model.Colors, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(_descriptor.AttribColorsLocation, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(_descriptor.AttribColorsLocation);
        }

        private void BindNormals()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, _descriptor.normalsBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(_gameObject.Model.Normals.Length * Vector3.SizeInBytes),
                _gameObject.Model.Normals, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(_descriptor.AttribNormalsLocation, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(_descriptor.AttribNormalsLocation);
        }
    }
}