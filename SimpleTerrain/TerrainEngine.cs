using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace SimpleTerrain
{
    class TerrainEngine : AbstractEngine
    {
        public Player Player { get; private set; }
        public RenderEngine MainRender { get; private set; }

        public TerrainEngine(int Width, int Height)
            : base(Width, Height)
        {
            Player = new Player();
            KeyHandler.KeyPress += HandleKeyPress;
            MainRender = new RenderEngine(Width, Height, Player);
        }


        public override void Click(Vector2 point)
        {
            
        }

        public override void Tick(long timeSlice, Vector2 dxdy)
        {
            KeyHandler.CheckKeys();

            Player.Tick(timeSlice, dxdy);

            var model = GetSimpleModel();

            MainRender.Draw(model, Player.FlashlightPosition);

        }

        private void HandleKeyPress(Common.Input.InputSignal signal)
        {
            Player.OnSignal(signal);
        }



       private SimpleModel GetSimpleModel()
        {
            var result = new SimpleModel()
            {
                TextureId = -1
            };

            result.Vertices = VERTICES;
            if (Colors == null)
            {
                var red = new Vector3(0.5f, 0, 0);
                Colors = Enumerable.Repeat(red, result.Vertices.Length).ToArray();
            }
            result.Colors = Colors;

            GetNormals(result);

            return result;
        }

        private void GetNormals(SimpleModel result)
        {
            var res = Enumerable.Repeat(Vector3.UnitY, result.Vertices.Length).ToArray();
            result.Normals = res;
        }

        private const float SIZE = 100f;

        private static Vector3[] Colors = null;

        public static Vector3[] VERTICES = {

        new Vector3(-SIZE,  -1, -SIZE),
        new Vector3( SIZE,  -1, -SIZE),
        new Vector3( SIZE,  -1,  SIZE),
        new Vector3( SIZE,  -1,  SIZE),
        new Vector3(-SIZE,  -1,  SIZE),
        new Vector3(-SIZE,  -1, -SIZE),
    };

    }
}
