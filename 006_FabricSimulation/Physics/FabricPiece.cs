using Common;
using OpenTK;

namespace FabricSimulation
{
    public class FabricPiece
    {
        public Vector3[] Normals { get; set; }
        public Vector3[] Colors { get; set; }
        public PointMass[,] PointsGrid;
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

            PointsGrid = masses;
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
            var l1 = PointsGrid.GetLength(0);
            var l2 = PointsGrid.GetLength(1);

            int k = 0;

            for (int i = 0; i < l1 - 1; i++)
            {
                for (int j = 0; j < l2 - 1; j++)
                {
                    vertices[k] = (PointsGrid[i, j].Location);
                    if (i > 0 && j > 0)
                    {
                        var norm = Vector3.Cross(
                            PointsGrid[i, j + 1].Location - PointsGrid[i, j - 1].Location,
                            PointsGrid[i + 1, j].Location - PointsGrid[i - 1, j].Location);
                        Normals[k] = norm;
                    }
                    else
                    {
                        Normals[k] = new Vector3(0, 0, 1);
                    }

                    k++;

                    vertices[k] = (PointsGrid[i, j + 1].Location);
                    if (i > 0 && j + 1 > 0 && j + 2 < l2)
                    {
                        var norm = Vector3.Cross(
                            PointsGrid[i, j + 2].Location - PointsGrid[i, j].Location,
                            PointsGrid[i + 1, j].Location - PointsGrid[i - 1, j].Location);
                        Normals[k] = norm;
                    }
                    else
                    {
                        Normals[k] = new Vector3(0, 0, 1);
                    }
                    k++;

                    vertices[k] = (PointsGrid[i + 1, j].Location);
                    if (i + 1 > 0 && j > 0 && i + 2 < l1)
                    {
                        var norm = Vector3.Cross(
                             PointsGrid[i, j + 1].Location - PointsGrid[i, j - 1].Location,
                             PointsGrid[i + 2, j].Location - PointsGrid[i, j].Location);
                        Normals[k] = norm;
                    }
                    else
                    {
                        Normals[k] = new Vector3(0, 0, 1);
                    }
                    k++;


                    vertices[k] = (PointsGrid[i, j + 1].Location);
                    if (i > 0 && j + 1 > 0 && j + 2 < l2)
                    {
                        var norm = Vector3.Cross(
                            PointsGrid[i, j + 2].Location - PointsGrid[i, j].Location,
                            PointsGrid[i + 1, j].Location - PointsGrid[i - 1, j].Location);
                        Normals[k] = norm;
                    }
                    else
                    {
                        Normals[k] = new Vector3(0, 0, 1);
                    }
                    k++;


                    vertices[k] = (PointsGrid[i + 1, j + 1].Location);
                    if (i + 2 < l1 && j + 2 < l2)
                    {
                        var norm = Vector3.Cross(
                            PointsGrid[i+1, j + 2].Location - PointsGrid[i + 1, j].Location,
                            PointsGrid[i + 2, j + 1].Location - PointsGrid[i, j + 1].Location);
                        Normals[k] = norm;
                    }
                    else
                    {
                        Normals[k] = new Vector3(0, 0, 1);
                    }
                    k++;


                    vertices[k] = (PointsGrid[i + 1, j].Location);
                    if (i + 1 > 0 && j > 0 && i + 2 < l1)
                    {
                        var norm = Vector3.Cross(
                             PointsGrid[i, j + 1].Location - PointsGrid[i, j - 1].Location,
                             PointsGrid[i + 2, j].Location - PointsGrid[i, j].Location);
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

            var l1 = PointsGrid.GetLength(0);
            var l2 = PointsGrid.GetLength(1);

            for (int k = 0; k < 2; k++)
            {
                for (int i = 0; i < l1; i++)
                {
                    for (int j = 0; j < l2; j++)
                    {
                        PointsGrid[i, j].SolveConstraints();
                    }
                }
            }

            for (int i = 0; i < l1; i++)
            {
                for (int j = 0; j < l2; j++)
                {
                    PointTick(PointsGrid[i, j], time);
                }
            }
        }

        private static void PointTick(PointMass point, long elapsed)
        {
            point.UpdatePhysics(elapsed);
        }
    }
}
