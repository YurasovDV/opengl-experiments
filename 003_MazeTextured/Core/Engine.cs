using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Common;
using Common.Input;
using OpenTK;
using MazeTextured.Core.Models;
using MazeTextured.Graphics;

namespace MazeTextured.Core
{
    class Engine
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public Player Player { get; set; }

        private RenderEngine renderEngine;

        public KeyHandler KeyHandler { get; set; }


        private List<SimpleModel> models;

        public List<SimpleModel> Models
        {
            get { return models; }
            set { models = value; }
        }

        public Maze World { get; set; }

        public int CurrentCell { get; set; }



        public Engine(int Width, int Height)
        {
            this.Width = Width;
            this.Height = Height;

            World = GetWorld();

            Player = new Player(NoIntersection);

            renderEngine = new Graphics.RenderEngine(Width, Height, Player);
            KeyHandler = new KeyHandler();
            KeyHandler.KeyPress += Player.OnSignal;

            Models = new List<SimpleModel>();
            

        }

        internal void Tick(long delta, Vector2 mouseDxDy)
        {
            KeyHandler.CheckKeys();

            Player.Tick(delta, mouseDxDy);
            var model = World.GetAsModel(Player.Position);

            // MarkPlayerCell(model);

            renderEngine.Render(model);
        }

        private void MarkPlayerCell(SimpleModel model)
        {
            if (CurrentCell != -1)
            {
                var cell = World.Cells[CurrentCell];
                if (cell.Lines != null && cell.Lines.Any())
                {
                    int wallsCount = 0;

                    for (int j = 0; j < CurrentCell; j++)
                    {
                        var c = World.Cells[j];
                        if (c.Lines != null)
                        {
                            wallsCount += c.Lines.Length;
                        }
                    }

                    wallsCount *= RenderEngine.TRIANGLES_IN_WALL;
                    for (int i = 0; i < RenderEngine.TRIANGLES_IN_WALL * cell.Lines.Length; i++)
                    {
                        model.Colors[wallsCount + i] = new Vector3(0, 1, 0);
                    }
                }
            }
        }

        private Maze GetWorld()
        {
            Maze result = null;
            XmlSerializer sr = new XmlSerializer(typeof(Maze));
            using (StreamReader rd = new StreamReader(@"Assets\Maze2.xml"))
            {
                result = (Maze)sr.Deserialize(rd);
            }

            return result;
        }



        private bool NoIntersection(Vector3 positionNext)
        {
           

            var xFrom = positionNext.X / Maze.shrinkCoef;
            var zFrom = positionNext.Z / Maze.shrinkCoef;

            int zShift = (int)zFrom / 15;
            int xShift = (int)xFrom / 15;
            int cellsInRow = (int)Math.Sqrt(World.Cells.Length);

            var index = zShift * cellsInRow + xShift;

            CurrentCell = -1;
            if (index >= 0 && index < World.Cells.Length)
            {
                try
                {
                    var cell = World.Cells[index];
                    CurrentCell = index;

                    if (cell.Lines != null)
                    {
                        bool any = cell.Lines.Any(l => Intersects(l, positionNext));
                        return !any || (positionNext.Y > Maze.height);
                    }
                }
                catch
                {

                }
            }
            
            return true;
        }


        private bool Intersects(Line line, Vector3 positionNext)
        {
            RectangleF point = new RectangleF(positionNext.X - 0.5f, positionNext.Z - 0.5f, 1, 1);

            RectangleF rectWall = new RectangleF();

            if (line.X0 == line.X1)
            {
                var yMin = Math.Min(line.Y0, line.Y1);
                rectWall = new RectangleF(line.X0 - Maze.wallWidth, yMin, 2 * Maze.wallWidth, Math.Max(line.Y0, line.Y1) - yMin);
            }
            else
            {
                var xMin = Math.Min(line.X0, line.X1);
                rectWall = new RectangleF(xMin, line.Y0 - Maze.wallWidth, Math.Max(line.X0, line.X1) - xMin, 2 * Maze.wallWidth);
            }

            var inters = rectWall.IntersectsWith(point);

            return inters;
        }

        internal void Click()
        {
            if (CurrentCell != -1)
            {
                Debug.WriteLine("Cell# {0} Player({1} ; {2})", CurrentCell, Player.Position.X, Player.Position.Z);
            }
        }
    }
}
