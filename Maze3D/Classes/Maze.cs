using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Maze3D.Classes.Graphics;
using OpenTK;

namespace Maze3D.Classes
{
    [XmlType(TypeName = "maze")]
    public class Maze
    {

        [XmlArray("cells")]
        public Cell[] Cells { get; set; }

        internal SimpleModel GetAsModel(Vector3 player)
        {
            if (cachedModel == null && Cells != null)
            {
                cachedModel = GenerateModel(Cells, player);
            }
            if (cachedModel == null)
            {
                return GetSquare();
            }

            return cachedModel;
        }

        private SimpleModel cachedModel = null;
        private float height = 4;

        private SimpleModel GenerateModel(Cell[] cells, Vector3 playerLookDirection)
        {
            SimpleModel result = new SimpleModel();

            List<Vector3> vertices = new List<Vector3>(cells.Length * 4 * 6);
            List<Vector3> normals = new List<Vector3>(cells.Length * 4 * 6);

            var matrLeft = Matrix4.CreateRotationY(MathHelper.PiOver2);
            var matrRight = Matrix4.CreateRotationY(-MathHelper.PiOver2);

            foreach (var cell in cells)
            {
                if (cell.Lines != null)
                {
                    foreach (var line in cell.Lines)
                    {
                        if (line.Name != null)
                        {
                            if (line.X0 == line.X1)
                            {
                                line.X0 *= 0.2f;
                                line.X1 = line.X0;
                                line.Y0 *= 0.2f;
                                line.Y1 *= 0.2f;


                            }
                            else if (line.Y1 == line.Y0)
                            {
                                line.Y0 *= 0.2f;
                                line.Y1 = line.Y0;

                                line.X0 *= 0.2f;
                                line.X1 *= 0.2f;
                            }
                            /*line.X0 *= 0.2f;
                            line.Y0 *= 0.2f;
                            line.X1 *= 0.2f;
                            line.Y1 *= 0.2f;*/

                            Vector3 v0 = new Vector3(line.X0, height, line.Y0);
                            Vector3 v1 = new Vector3(line.X0, 0, line.Y0);
                            Vector3 v2 = new Vector3(line.X1, 0, line.Y1);

                            vertices.Add(v0);
                            vertices.Add(v1);
                            vertices.Add(v2);

                            v0 = new Vector3(line.X0, height, line.Y0);
                            v1 = new Vector3(line.X1, 0, line.Y1);
                            v2 = new Vector3(line.X1, height, line.Y1);

                            vertices.Add(v0);
                            vertices.Add(v1);
                            vertices.Add(v2);

                            Vector3 lineDirection = new Vector3(line.X0 - line.X1, 0, line.Y0 - line.Y1);

                            Vector3 normalLeft = new Vector3(lineDirection);
                            Vector3 normalRight = new Vector3(lineDirection);

                            normalLeft = Vector3.TransformVector(normalLeft, matrLeft);
                            normalRight = Vector3.TransformVector(normalRight, matrRight);

                            var left = Vector3.Dot(playerLookDirection, normalLeft);
                            var right = Vector3.Dot(playerLookDirection, normalRight);
                            Vector3 normal;
                            if (left < right)
                            {
                                normal = normalLeft;
                            }
                            else
                            {
                                normal = normalRight;
                            }
                            normal.Normalize();
                            normals.Add(normal);
                            normals.Add(normal);
                            normals.Add(normal);

                            normals.Add(normal);
                            normals.Add(normal);
                            normals.Add(normal);
                        }
                    }
                }
            }

            Vector3 red = new Vector3(1, 0, 0);
            var colors = Enumerable.Repeat<Vector3>(red, vertices.Count).ToArray();


            result.Vertices = vertices.ToArray();
            result.Color = colors.ToArray();
            result.Normals = normals.ToArray();

            return result;
        }



        private SimpleModel GetSquare()
        {
            SimpleModel model = new SimpleModel();
            model.Vertices = new Vector3[]
            {
                new Vector3(0, 0, 0),
                new Vector3(10, 0, 0),
                new Vector3(10, 10, 0),

                new Vector3(0, 0, 0),
                new Vector3(10, 10, 0),
                new Vector3(0, 10, 0),
            };

            model.Color = new Vector3[]
            {
                new Vector3(100, 0, 0),
                new Vector3(100, 0, 0),
                new Vector3(100, 0, 0),
                new Vector3(100, 0, 0),
                new Vector3(100, 0, 0),
                new Vector3(100, 0, 0),
            };

            model.Normals = new Vector3[]
            {
                new Vector3(0, 0, 1),
                new Vector3(0, 0, 1),
                new Vector3(0, 0, 1),
                           
                new Vector3(0, 0, 1),
                new Vector3(0, 0, 1),
                new Vector3(0, 0, 1),
            };
            return model;
        }
    }
}
