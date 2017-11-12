using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Common;
using OpenTK;
using MazeTextured.Core.Utils;
using MazeTextured.Graphics;

namespace MazeTextured.Core.Models
{
    [XmlType(TypeName = "maze")]
    public class Maze
    {

        [XmlArray("cells")]
        public Cell[] Cells { get; set; }



        [XmlIgnore]
        private SimpleModel cachedModel = null;

        [XmlIgnore]
        public const float height = 5;

        [XmlIgnore]
        //public const float shrinkCoef = 0.4f;   
        public const float shrinkCoef = 0.6f;

        [XmlIgnore]
        public const float wallWidth = 0.5f;


        public SimpleModel GetAsModel(Vector3 player)
        {
            if (cachedModel == null && Cells != null)
            {
                cachedModel = GenerateModel(Cells);
            }
            if (cachedModel == null)
            {
                throw new Exception("maze asset is not found");
            }

            Vector3 red = new Vector3(1, 0, 0);
            var colors = Enumerable.Repeat(red, cachedModel.Vertices.Length).ToArray();
            cachedModel.Colors = colors;
            return cachedModel;
        }


        private void StoreAsObj(SimpleModel model)
        {
            using (StreamWriter w = new StreamWriter("outputModel.obj"))
            {
                w.WriteLine("o maze.001");
                foreach (var v in model.Vertices)
                {
                    w.WriteLine("v {0} {1} {2}", v.X, v.Y, v.Z);
                }

                foreach (var vt in model.TextureCoordinates)
                {
                    w.WriteLine("vt {0} {1}", vt.X, vt.Y);
                }

                foreach (var n in model.Normals)
                {
                    w.WriteLine("vn {0} {1} {2}", n.X, n.Y, n.Z);
                }

                for (int i = 0; i < model.Vertices.Length; i += 3)
                {
                    // f 57/3/9 61/4/9 62/1/9 58/2/9
                    w.Write("f ");
                    for (int j = 0; j < 3; j++)
                    {
                        w.Write("{0}/{1}/{2} ", i + j + 1, i + j + 1, i + j + 1);
                    }
                    w.WriteLine();
                }
                w.WriteLine();

            }
        }

        private void GetData(Cell[] cells)
        {
            float maxX = 0;
            float maxZ = 0;

            foreach (var cell in cells)
            {
                if (cell.Lines != null)
                {
                    foreach (var line in cell.Lines)
                    {
                        maxX = (float)MathHelperMINE.Max(maxX, line.X0, line.X1);
                        maxZ = (float)MathHelperMINE.Max(maxZ, line.Y0, line.Y1);
                    }
                }
            }

            Debug.WriteLine("x = " + maxX);
            Debug.WriteLine("z = " + maxZ);

        }


        private Matrix4 matrClockWise = Matrix4.CreateRotationY(MathHelper.PiOver2);
        private Matrix4 matrCounter = Matrix4.CreateRotationY(-MathHelper.PiOver2);


        private SimpleModel GenerateModel(Cell[] cells)
        {
            SimpleModel result = new SimpleModel();

            List<Vector3> vertices = new List<Vector3>(cells.Length * 4 * 6);
            List<Vector3> normals = new List<Vector3>(cells.Length * 4 * 6);

            foreach (var cell in cells)
            {
                if (cell.Lines != null)
                {
                    foreach (var line in cell.Lines)
                    {
                        if (line.Name != null)
                        {
                            GetWall(vertices, normals, line);
                        }
                    }
                }
            }

            Vector3 red = new Vector3(1, 0, 0);
            var colors = Enumerable.Repeat<Vector3>(red, vertices.Count).ToArray();

            result.Vertices = vertices.ToArray();
            result.Colors = colors.ToArray();
            result.Normals = normals.ToArray();

            var mgr = new Graphics.TextureManager();
            result.TextureId = mgr.LoadTexture(@"Assets\Textures\brick.jpg", 1024, 1024);
            result.TextureCoordinates = mgr.GetTextureCoordinates(result.Vertices);
            return result;
        }


        private void GetWall(List<Vector3> vertices, List<Vector3> normals, Line line)
        {
            // перпендикулярно oX
            if (line.X0 == line.X1)
            {
                line.X0 *= shrinkCoef;
                line.X1 = line.X0;

                line.Y0 = line.Y0 * shrinkCoef;
                line.Y1 = line.Y1 * shrinkCoef;

                DubByX(vertices, normals, line);
            }

            // перпендикулярно oY
            else if (line.Y1 == line.Y0)
            {
                line.Y0 *= shrinkCoef;
                line.Y1 = line.Y0;

                line.X0 *= shrinkCoef;
                line.X1 *= shrinkCoef;

                DubByY(vertices, normals, line);
            }
        }

        // y0 == y1
        private void DubByY(List<Vector3> vertices, List<Vector3> normals, Line line)
        {
            Vector3 lineDirection = new Vector3(line.X0 - line.X1, 0, line.Y0 - line.Y1);

            #region бок 1

            float toLessY = line.Y0 - wallWidth;

            Vector3 v0 = new Vector3(line.X0, height, toLessY);
            Vector3 v1 = new Vector3(line.X0, 0, toLessY);
            Vector3 v2 = new Vector3(line.X1, 0, toLessY);

            vertices.Add(v0);
            vertices.Add(v1);
            vertices.Add(v2);

            v0 = new Vector3(line.X1, 0, toLessY);
            v1 = new Vector3(line.X1, height, toLessY);
            v2 = new Vector3(line.X0, height, toLessY);


            vertices.Add(v0);
            vertices.Add(v1);
            vertices.Add(v2);

            Vector3 normalLeft = new Vector3(lineDirection);
            normalLeft = Vector3.Transform(normalLeft, matrCounter);
            for (int i = 0; i < 6; i++)
            {
                normals.Add(normalLeft);
            }

            #endregion

            #region бок 2


            float toBiggerY = line.Y0 + wallWidth;

            v0 = new Vector3(line.X0, height, toBiggerY);
            v1 = new Vector3(line.X0, 0, toBiggerY);
            v2 = new Vector3(line.X1, 0, toBiggerY);

            vertices.Add(v0);
            vertices.Add(v1);
            vertices.Add(v2);

            v0 = new Vector3(line.X1, 0, toBiggerY);
            v1 = new Vector3(line.X1, height, toBiggerY);
            v2 = new Vector3(line.X0, height, toBiggerY);

            vertices.Add(v0);
            vertices.Add(v1);
            vertices.Add(v2);


            Vector3 normalRight = new Vector3(lineDirection);
            normalRight = Vector3.Transform(normalRight, matrClockWise);
            for (int i = 0; i < 6; i++)
            {
                normals.Add(normalRight);
            }

            #endregion


            // торец 1


            v0 = new Vector3(line.X0, height, line.Y0 - wallWidth);
            v1 = new Vector3(line.X0, 0, line.Y0 - wallWidth);
            v2 = new Vector3(line.X0, 0, line.Y0 + wallWidth);



            vertices.Add(v0);
            vertices.Add(v1);
            vertices.Add(v2);


            v0 = new Vector3(line.X0, 0, line.Y0 + wallWidth);
            v1 = new Vector3(line.X0, height, line.Y0 + wallWidth);
            v2 = new Vector3(line.X0, height, line.Y0 - wallWidth);

            vertices.Add(v0);
            vertices.Add(v1);
            vertices.Add(v2);


            Vector3 normalFront = new Vector3(lineDirection);
            for (int i = 0; i < 6; i++)
            {
                normals.Add(normalFront);
            }

            // торец 2
            v0 = new Vector3(line.X1, height, line.Y0 - wallWidth);
            v1 = new Vector3(line.X1, 0, line.Y0 - wallWidth);
            v2 = new Vector3(line.X1, 0, line.Y0 + wallWidth);


            vertices.Add(v0);
            vertices.Add(v1);
            vertices.Add(v2);


            v0 = new Vector3(line.X1, 0, line.Y0 + wallWidth);
            v1 = new Vector3(line.X1, height, line.Y0 + wallWidth);
            v2 = new Vector3(line.X1, height, line.Y0 - wallWidth);

            vertices.Add(v0);
            vertices.Add(v1);
            vertices.Add(v2);


            Vector3 normalBack = new Vector3(lineDirection);
            normalBack = Vector3.Multiply(normalBack, new Vector3(-1, 1, 1));
            for (int i = 0; i < 6; i++)
            {
                normals.Add(normalBack);
            }

            // верх                               
            v0 = new Vector3(line.X0, height, line.Y0 + wallWidth);
            v1 = new Vector3(line.X0, height, line.Y0 - wallWidth);
            v2 = new Vector3(line.X1, height, line.Y0 - wallWidth);


            vertices.Add(v0);
            vertices.Add(v1);
            vertices.Add(v2);


            v0 = new Vector3(line.X1, height, line.Y0 - wallWidth);
            v1 = new Vector3(line.X1, height, line.Y0 + wallWidth);
            v2 = new Vector3(line.X0, height, line.Y0 + wallWidth);

            vertices.Add(v0);
            vertices.Add(v1);
            vertices.Add(v2);

            for (int i = 0; i < 6; i++)
            {
                normals.Add(Vector3.UnitY);
            }
        }

        // x0 == x1
        private void DubByX(List<Vector3> vertices, List<Vector3> normals, Line line)
        {
            Vector3 lineDirection = new Vector3(line.X0 - line.X1, 0, line.Y0 - line.Y1);

            // бок 1
            float toLessX = line.X0 - wallWidth;

            Vector3 v0 = new Vector3(toLessX, height, line.Y0);
            Vector3 v1 = new Vector3(toLessX, 0, line.Y0);
            Vector3 v2 = new Vector3(toLessX, 0, line.Y1);

            vertices.Add(v0);
            vertices.Add(v1);
            vertices.Add(v2);

            v0 = new Vector3(toLessX, 0, line.Y1);
            v1 = new Vector3(toLessX, height, line.Y1);
            v2 = new Vector3(toLessX, height, line.Y0);

            vertices.Add(v0);
            vertices.Add(v1);
            vertices.Add(v2);

            Vector3 normalLeft = new Vector3(lineDirection);
            normalLeft = Vector3.Transform(normalLeft, matrClockWise);
            for (int i = 0; i < 6; i++)
            {
                normals.Add(normalLeft);
            }

            // бок 2

            float toBiggerX = line.X0 + wallWidth;

            v0 = new Vector3(toBiggerX, height, line.Y0);
            v2 = new Vector3(toBiggerX, 0, line.Y1);
            v1 = new Vector3(toBiggerX, 0, line.Y0);

            vertices.Add(v0);
            vertices.Add(v1);
            vertices.Add(v2);


            v0 = new Vector3(toBiggerX, 0, line.Y1);
            v1 = new Vector3(toBiggerX, height, line.Y1);
            v2 = new Vector3(toBiggerX, height, line.Y0);

            vertices.Add(v0);
            vertices.Add(v1);
            vertices.Add(v2);


            Vector3 normalRight = new Vector3(lineDirection);
            normalRight = Vector3.Transform(normalRight, matrCounter);
            for (int i = 0; i < 6; i++)
            {
                normals.Add(normalRight);
            }

            // торец 1

            v0 = new Vector3(line.X0 - wallWidth, 0, line.Y0);
            v1 = new Vector3(line.X0 + wallWidth, 0, line.Y0);
            v2 = new Vector3(line.X0 + wallWidth, height, line.Y0);

            vertices.Add(v0);
            vertices.Add(v1);
            vertices.Add(v2);

            v0 = new Vector3(line.X0 + wallWidth, height, line.Y0);
            v1 = new Vector3(line.X0 - wallWidth, height, line.Y0);
            v2 = new Vector3(line.X0 - wallWidth, 0, line.Y0);

            vertices.Add(v0);
            vertices.Add(v1);
            vertices.Add(v2);

            Vector3 normalFront = new Vector3(lineDirection);
            for (int i = 0; i < 6; i++)
            {
                normals.Add(normalFront);
            }

            // торец 2
            v0 = new Vector3(line.X1 - wallWidth, 0, line.Y1);
            v1 = new Vector3(line.X1 + wallWidth, 0, line.Y1);
            v2 = new Vector3(line.X1 + wallWidth, height, line.Y1);

            vertices.Add(v0);
            vertices.Add(v1);
            vertices.Add(v2);


            v0 = new Vector3(line.X1 + wallWidth, height, line.Y1);
            v1 = new Vector3(line.X1 - wallWidth, height, line.Y1);
            v2 = new Vector3(line.X1 - wallWidth, 0, line.Y1);

            vertices.Add(v0);
            vertices.Add(v1);
            vertices.Add(v2);

            Vector3 normalBack = new Vector3(lineDirection);
            //normalBack = Vector3.Transform(normalBack, matrClockWise);
            normalBack = Vector3.Multiply(normalBack, new Vector3(1, -1, 1));
            for (int i = 0; i < 6; i++)
            {
                normals.Add(normalBack);
            }


            // верх                               
            v0 = new Vector3(line.X0 + wallWidth, height, line.Y0);
            v1 = new Vector3(line.X0 - wallWidth, height, line.Y0);
            v2 = new Vector3(line.X0 - wallWidth, height, line.Y1);

            vertices.Add(v0);
            vertices.Add(v1);
            vertices.Add(v2);


            v0 = new Vector3(line.X0 - wallWidth, height, line.Y1);
            v1 = new Vector3(line.X0 + wallWidth, height, line.Y1);
            v2 = new Vector3(line.X0 + wallWidth, height, line.Y0);


            vertices.Add(v0);
            vertices.Add(v1);
            vertices.Add(v2);

            for (int i = 0; i < 6; i++)
            {
                normals.Add(Vector3.UnitY);
            }
        }
    }
}
