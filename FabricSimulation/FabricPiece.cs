using System.Diagnostics;
using Common;
using OpenTK;

namespace FabricSimulation
{
    public class FabricPiece
    {
        public Vector3[] Normals { get; set; }
        public Vector3[] Colors { get; set; }
        public PointMass[,] Points;
        public Vector3[] vertices;

        public FabricPiece(Vector3[,] points)
        {
            var l1 = points.GetLength(0);
            var l2 = points.GetLength(1);

            var masses = new PointMass[l1, l2];
            for (int x = 0; x < l1; x++)
            {
                for (int y = 0; y < l2; y++)
                {
                    bool firstRow = x == 0;
                    // Debug.WriteLine(points[x, y].ToString());
                    masses[x, y] = new PointMass(points[x, y], firstRow);

                    if (x != 0)
                    {
                        var constraint = new Constraint(masses[x, y], masses[x - 1, y], FabricSimulationEngine.NormalDistance);
                        masses[x, y].Constraints.Add(constraint);
                    }

                    if (y != 0)
                    {
                        var constraint = new Constraint(masses[x, y], masses[x, y - 1], FabricSimulationEngine.NormalDistance);
                        masses[x, y].Constraints.Add(constraint);
                    }
                }
            }

            Points = masses;
            const int verticesPerPoint = 6;

            Normals = new Vector3[l1 * l2 * verticesPerPoint];
            Colors = new Vector3[l1 * l2 * verticesPerPoint];

            vertices = new Vector3[l1 * l2 * verticesPerPoint];
            int l = 0;
            for (int i = 0; i < Normals.Length; i++)
            {
                //var color = l < 3 ? new Vector3(0, 0, 100) : new Vector3(0, 80, 100);
                var color = new Vector3(0, 0, 100);
                Colors[i] = color;
                l = (l + 1) % 6;
            }
        }


        public SimpleModel GetAsModel()
        {
            var l1 = Points.GetLength(0);
            var l2 = Points.GetLength(1);

            int k = 0;

            for (int i = 0; i < l1 - 1; i++)
            {
                for (int j = 0; j < l2 - 1; j++)
                {
                    vertices[k] = (Points[i, j].Location);
                    if (i > 0 && j > 0)
                    {
                        var norm = Vector3.Cross(
                            Points[i, j + 1].Location - Points[i, j - 1].Location,
                            Points[i + 1, j].Location - Points[i - 1, j].Location);
                        Normals[k] = norm;
                    }
                    else
                    {
                        Normals[k] = new Vector3(0, 0, 1);
                    }

                    k++;

                    vertices[k] = (Points[i, j + 1].Location);
                    if (i > 0 && j + 1 > 0 && j + 2 < l2)
                    {
                        var norm = Vector3.Cross(
                            Points[i, j + 2].Location - Points[i, j].Location,
                            Points[i + 1, j].Location - Points[i - 1, j].Location);
                        Normals[k] = norm;
                    }
                    else
                    {
                        Normals[k] = new Vector3(0, 0, 1);
                    }
                    k++;

                    vertices[k] = (Points[i + 1, j].Location);
                    if (i + 1 > 0 && j > 0 && i + 2 < l1)
                    {
                        var norm = Vector3.Cross(
                             Points[i, j + 1].Location - Points[i, j - 1].Location,
                             Points[i + 2, j].Location - Points[i, j].Location);
                        Normals[k] = norm;
                    }
                    else
                    {
                        Normals[k] = new Vector3(0, 0, 1);
                    }
                    k++;


                    vertices[k] = (Points[i, j + 1].Location);
                    if (i > 0 && j + 1 > 0 && j + 2 < l2)
                    {
                        var norm = Vector3.Cross(
                            Points[i, j + 2].Location - Points[i, j].Location,
                            Points[i + 1, j].Location - Points[i - 1, j].Location);
                        Normals[k] = norm;
                    }
                    else
                    {
                        Normals[k] = new Vector3(0, 0, 1);
                    }
                    k++;


                    vertices[k] = (Points[i + 1, j + 1].Location);
                    if (i + 2 < l1 && j + 2 < l2)
                    {
                        var norm = Vector3.Cross(
                            Points[i+1, j + 2].Location - Points[i + 1, j].Location,
                            Points[i + 2, j + 1].Location - Points[i, j + 1].Location);
                        Normals[k] = norm;
                    }
                    else
                    {
                        Normals[k] = new Vector3(0, 0, 1);
                    }
                    k++;


                    vertices[k] = (Points[i + 1, j].Location);
                    if (i + 1 > 0 && j > 0 && i + 2 < l1)
                    {
                        var norm = Vector3.Cross(
                             Points[i, j + 1].Location - Points[i, j - 1].Location,
                             Points[i + 2, j].Location - Points[i, j].Location);
                        Normals[k] = norm;
                    }
                    else
                    {
                        Normals[k] = new Vector3(0, 0, 1);
                    }
                    k++;
                }
            }

            var model = new SimpleModel()
            {
                Vertices = vertices,
                Colors = Colors,
                Normals = Normals
            };

            return model;
        }

        internal void Tick(long time)
        {

            var l1 = Points.GetLength(0);
            var l2 = Points.GetLength(1);

            for (int k = 0; k < 2; k++)
            {
                for (int i = 0; i < l1; i++)
                {
                    for (int j = 0; j < l2; j++)
                    {
                        Points[i, j].SolveConstraints();
                    }
                }
            }

            for (int i = 0; i < l1; i++)
            {
                for (int j = 0; j < l2; j++)
                {
                    PointTick(Points[i, j], time);
                }
            }



            /*
            Action<PointMass, int, int> solve = (p, i, j) => p.SolveConstraints();

            for (int i = 0; i < 2; i++)
            {
                VisitVertices(solve);
            }

            Action<PointMass, int, int> a = (p, i, j) => PointTick(p, time);
            VisitVertices(a);*/
        }

        private static void PointTick(PointMass point, long elapsed)
        {
            point.UpdatePhysics(elapsed);
        }

        //private void VisitVertices(Action<PointMass, int, int> action, int boundX = 0, int boundY = 0)
        //{
        //    var l1 = Points.GetLength(0);
        //    var l2 = Points.GetLength(1);

        //    for (int i = 0; i < l1 - boundX; i++)
        //    {
        //        for (int j = 0; j < l2 - boundY; j++)
        //        {
        //            action(Points[i, j], i, j);
        //        }
        //    }
        //}
    }
}
