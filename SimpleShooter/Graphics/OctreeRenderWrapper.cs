using System;
using Common;
using OcTreeLibrary;
using OpenTK.Graphics.OpenGL4;
using SimpleShooter.Core;

namespace SimpleShooter.Graphics
{
    class OctreeRenderWrapper : IRenderWrapper
    {
        private SimpleModel _model = null;
        private IRenderWrapper _wrapper = null;

        public OctreeRenderWrapper(OcTree tree)
        {
            var visitor = new TreeVisitor(tree);
            _model = visitor.GetModel();
            _wrapper = new RenderWrapper(new GameObject(_model, ShaderKind));
        }

        public PrimitiveType RenderType
        {
            get
            {
                return PrimitiveType.Lines;
            }

            set
            {
                
            }
        }

        public ShadersNeeded ShaderKind
        {
            get
            {
                return ShadersNeeded.Line;
            }
        }

        public int VerticesCount
        {
            get
            {
                return _model.Vertices.Length;
            }
        }

        public void Bind(Camera camera, Level level)
        {
            _wrapper.Bind(camera, level);
        }

        public ShaderProgramDescriptor GetDescriptor()
        {
            return _wrapper.GetDescriptor();
        }
    }
}
