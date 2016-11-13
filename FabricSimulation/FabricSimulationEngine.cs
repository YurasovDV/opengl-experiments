using System;
using System.Diagnostics;
using Common;
using Common.Input;
using OpenTK;
using OpenTK.Input;

namespace FabricSimulation
{
    public class FabricSimulationEngine
    {
        public const int PieceWidth = 120;
        public const int PieceHeight = 30;

        public const float NormalDistance = 0.1f;

        public Vector2 MouseDownPosition { get; set; }
        public Vector2 MouseUpPosition { get; set; }

        public FabricPiece Piece { get; set; }

        public RenderEngine RenderEngine { get; set; }

        public AbstractPlayer Player { get; set; }
        public long LeftOverTime { get; private set; }

        KeyHandler KeyHandler;

        public FabricSimulationEngine(int width, int height)
        {
            Player = new PinnedPlayer();
            RenderEngine = new RenderEngine(width, height, Player);

            KeyHandler = new KeyHandler();
            KeyHandler.KeyPress += Player.OnSignal;

            // то, что не использовали на прошлом шаге симуляции
            LeftOverTime = 0;

            var iShift = (int)(PieceHeight / 0.5f);
            var jShift = PieceWidth / 2;

            var vertices = new Vector3[PieceHeight, PieceWidth];

            const float cellSize = NormalDistance;

            for (var i = 0; i < PieceHeight; i++)
            {
                for (var j = 0; j < PieceWidth; j++)
                {
                    var v = new Vector3((j - jShift) * -cellSize, (i - iShift) * -cellSize, 0);
                    vertices[i, j] = v;

                }
            }

            Piece = new FabricPiece(vertices);
        }

        internal void OnMouseDown(MouseButtonEventArgs e)
        {
            MouseDownPosition = new Vector2(e.Position.X, e.Position.Y);
        }

        internal void OnMouseUp(MouseButtonEventArgs e)
        {
            // var unTran = new Vector3(e.Position.X, e.Position.Y, 0);

            float x = 2.0f * e.Position.X / RenderEngine.Width - 1;
            float y = -2.0f * e.Position.Y / RenderEngine.Height + 1;

            var upPos = new Vector4(x, y, 0, 1.0f);
            var m = Matrix4.Invert(RenderEngine.Projection * RenderEngine.ModelView) * Matrix4.CreateRotationY(MathHelper.Pi);
            //var m = RenderEngine.ModelViewProjection * Matrix4.CreateRotationY(MathHelper.Pi);
            var point = Vector4.Transform(upPos, m);


            point.W = 1.0f / point.W;
          
            point.X *= point.W;
            point.Y *= point.W;
            point.Z *= point.W;


            // point = new Vector3(point.X * RenderEngine.Width * 0.05f, point.Y * RenderEngine.Height * 0.05f, point.Z);


            var d = new Vector3(point.X, point.Y, point.Z);
            float min = float.MaxValue;
            PointMass closest = null;
            foreach (var mass in Piece.Points)
            {
                var len = (mass.Location - d).LengthSquared;
                if (len < min)
                {
                    closest = mass;
                    min = len;
                }
            }

            //var maxX = PieceWidth* NormalDistance * 0.5;

            //Debug.WriteLine(upPos);
            Debug.WriteLine(point);
            // Debug.WriteLine(Piece.Points[PieceHeight - 1, PieceWidth - 1].Location);
            //Debug.WriteLine(Piece.Points[PieceHeight / 2, PieceWidth / 2].Location);
            Debug.WriteLine(Piece.Points[0, 0].Location);
            Debug.WriteLine(Piece.Points[PieceHeight - 1, PieceWidth - 1].Location);

            Debug.WriteLine("");

            if (closest != null)
            {
                //var force = new Vector3(0, 0, -1000000f) / (1000 / 16.0f);
                var force = new Vector3(0, 0, -1000000f) / (1000 / 16.0f);
                Piece.Points[PieceHeight / 2, PieceWidth / 2].AddForce(force);
            }
            /*
            
            
            (0,1935055; 0,1963296;  0
                                    
            (-0,1908401; 0,1883856; 0
                                    
            (-0,1908401; -0,1498006 0
                                    
            (0,1892409; -0,15434;  0
            
            */

            Player.OnSignal(InputSignal.MOUSE_CLICK);
        }

        internal void Tick(long time)
        {
            KeyHandler.CheckKeys();
            Player.Tick(time, new Vector2());

            time += LeftOverTime;
            var chunks = (long)Math.Floor(time / 16m);
            for (int i = 0; i < chunks; i++)
            {
                Piece.Tick(16);
            }

            // Debug.WriteLine(time);

            LeftOverTime = time - chunks * 16;

            var model = Piece.GetAsModel();

            RenderEngine.Render(new[] { model });
        }


    }
}
