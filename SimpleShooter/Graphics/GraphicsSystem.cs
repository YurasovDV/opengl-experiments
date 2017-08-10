using System.Collections.Generic;
using System.Linq;
using OpenTK;
using SimpleShooter.Core;
using SimpleShooter.PlayerControl;
using ClearBufferMask = OpenTK.Graphics.OpenGL4.ClearBufferMask;
using GL = OpenTK.Graphics.OpenGL4.GL;

namespace SimpleShooter.Graphics
{
    internal class GraphicsSystem
    {
        private Camera Camera { get; set; }

        private SkyBoxRenderer _skybox;

        private MarkController _mark;

        private Vector3[] GameObjectsLineVertices;
        private Vector3[] GameObjectsLineColors;

        private Vector3[] GameObjectsTextureLessVertices;
        private Vector3[] GameObjectsTextureLessColors;
        private Vector3[] GameObjectsTextureLessNormals;

        private Vector3[] GameObjectsTextureLessNoLightVertices;
        private Vector3[] GameObjectsTextureLessNoLightColors;


        public GraphicsSystem(int width, int height)
        {
            InitGraphics(width, height);
        }

        private void InitGraphics(int width, int height)
        {
            float aspect = (float) width / height;
            GL.Viewport(0, 0, width, height);
            var projection = Matrix4.CreatePerspectiveFieldOfView(0.5f, aspect, 0.1f, 200);
            Camera = new Camera(projection);

            _skybox = new SkyBoxRenderer(70);
            _mark = new MarkController(width, height);
        }

        internal void Render(ObjectsGrouped objects, Level level)
        {
            Camera.RebuildMatrices(level.Player);

            GL.ClearColor(0, 0, 0.0f, 1);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.DepthTest);

            _skybox.Render(level.Player, Camera);

            GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.DepthTest);

            RenderByTypes(objects, level);

           // GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.DepthTest);

            RenderMark(level);

            GL.Flush();
        }

        private void RenderMark(Level level)
        {
            _mark.Render(level.Player);
        }

        private void RenderByTypes(ObjectsGrouped objects, Level level)
        {
            RenderLines(objects.GameObjectsLine, level);
            RenderTextureless(objects.GameObjectsTextureLess, level);
            RenderTexturelessNoLight(objects.GameObjectsTextureLessNoLight, level);
            RenderModels(objects.GameObjectsSimpleModel, level);
        }

        private void RenderModels(List<GameObjectDescriptor> simpleModels, Level level)
        {
            foreach (var obj in simpleModels)
            {
                obj.Bind(Camera, level);
                GL.DrawArrays(obj.RenderType, 0, obj.VerticesCount);
            }
        }

        private void RenderTexturelessNoLight(List<GameObjectDescriptor> textureLessNoLight, Level level)
        {
            BindColorsAndVertices(textureLessNoLight, level, ref GameObjectsTextureLessNoLightVertices, ref GameObjectsTextureLessNoLightColors);
        }

        private void RenderTextureless(List<GameObjectDescriptor> textureLess, Level level)
        {

            BindColorsAndVerticesAndNormals(textureLess, level, ref GameObjectsTextureLessVertices, ref GameObjectsTextureLessColors, ref GameObjectsTextureLessNormals);

        }

        private void RenderLines(List<GameObjectDescriptor> lines, Level level)
        {
            BindColorsAndVertices(lines, level, ref GameObjectsLineVertices, ref GameObjectsLineColors);
        }

        private void BindColorsAndVertices(List<GameObjectDescriptor> objects, Level level, ref Vector3[] verticesMember, ref Vector3[] colorsMember)
        {
            var descriptor = objects.FirstOrDefault()?.GetDescriptor();
            if (descriptor != null)
            {
                GL.UseProgram(descriptor.ProgramId);

                int vCount = GetVerticesCount(objects);

                if (verticesMember == null || verticesMember.Length < vCount)
                {
                    verticesMember = new Vector3[vCount];
                    colorsMember = new Vector3[vCount];
                }

                int pointer = 0;

                foreach (var o in objects)
                {
                    var objectVertices = o.GameIdentity.Model.Vertices;
                    var objectColors = o.GameIdentity.Model.Colors;
                    for (int i = 0; i < objectVertices.Length; i++)
                    {
                        verticesMember[pointer] = objectVertices[i];
                        colorsMember[pointer] = objectColors[i];
                        pointer++;
                    }
                }

                RenderWrapper.BindUniforms(descriptor, Camera, level.LightPosition);
                RenderWrapper.BindVertices(descriptor, verticesMember);
                RenderWrapper.BindColors(descriptor, colorsMember);

                GL.DrawArrays(objects[0].RenderType, 0, vCount);
            }
        }

        private void BindColorsAndVerticesAndNormals(List<GameObjectDescriptor> objects, Level level, ref Vector3[] verticesMember, ref Vector3[] colorsMember, ref Vector3[] normalsMember)
        {
            var descriptor = objects.FirstOrDefault()?.GetDescriptor();
            if (descriptor != null)
            {
                GL.UseProgram(descriptor.ProgramId);

                int vCount = objects.Aggregate(0, (acc, d) => d.VerticesCount + acc);

                if (verticesMember == null || verticesMember.Length < vCount)
                {
                    verticesMember = new Vector3[vCount];
                    colorsMember = new Vector3[vCount];
                    normalsMember = new Vector3[vCount];
                }

                int pointer = 0;

                foreach (var o in objects)
                {
                    var objectVertices = o.GameIdentity.Model.Vertices;
                    var objectColors = o.GameIdentity.Model.Colors;
                    var objectNormals = o.GameIdentity.Model.Normals;
                    for (int i = 0; i < objectVertices.Length; i++)
                    {
                        verticesMember[pointer] = objectVertices[i];
                        colorsMember[pointer] = objectColors[i];
                        normalsMember[pointer] = objectNormals[i];
                        pointer++;
                    }
                }

                RenderWrapper.BindUniforms(descriptor, Camera, level.LightPosition);
                RenderWrapper.BindVertices(descriptor, verticesMember);
                RenderWrapper.BindColors(descriptor, colorsMember);
                RenderWrapper.BindNormals(descriptor, normalsMember);

                GL.DrawArrays(objects[0].RenderType, 0, vCount);
            }
        }


        internal void Render(IRenderWrapper obj, Level level)
        {
            obj.Bind(Camera, level);
            GL.DrawArrays(obj.RenderType, 0, obj.VerticesCount);
        }

        private static int GetVerticesCount(List<GameObjectDescriptor> objects)
        {
            // objects.Aggregate(0, (acc, d) => d.VerticesCount + acc);
            int res = 0;
            for (int i = 0; i < objects.Count; i++)
            {
                res += objects[i].VerticesCount;
            }
            return res;
        }
    }
}