using System;
using System.Collections.Generic;
using Common;
using Common.Utils;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace SimpleTerrain
{
    class HeightMap
    {
        public int MapSize { get { return Colors.GetLength(0); } }

        public bool TryGetValue(int x, int z, out float result)
        {
            result = -110;
            bool inside = 
                x >= 0 && x < MapSize
                && z >= 0 && z < MapSize;

            if (inside)
            {
                result = this[x, z];
            }

            return inside;
        }

        public float this[int x, int z]
        {
            get
            {
                return map[x, z];
            }
        }

        private float[,] map;
        private Vector3[,] Colors { get; set; }


        private Vector3[] points;
        private Vector3[] normals;
        private Vector3[] colors;

        public HeightMap(float[,] map)
        {
            this.map = map;

            var l1 = map.GetLength(0);
            var l2 = map.GetLength(1);

            Colors = new Vector3[l1, l2];
            for (int i = 0; i < l1; i++)
            {
                for (int j = 0; j < l2; j++)
                {
                    var v = new Vector3(1, 1, 0);
                    Colors[i, j] = v;
                }
            }
        }

        internal void RebuildVerticesAccordingly(PrimitiveType renderMode)
        {
            points = GetPoints(renderMode);
        }

        private Vector3[] GetPoints(PrimitiveType renderMode)
        {
            if (renderMode == PrimitiveType.Triangles)
            {
                return FillVerticesAndNormalsAsTriangles();
            }
            if (renderMode == PrimitiveType.Lines)
            {
                return FillVerticesAndNormalsAsLines();
            }

            throw new ArgumentException("incorrect renderMode", "renderMode");
        }

        public static HeightMap Create(int size)
        {
            float[,] map = new float[size + 1, size + 1];

            Generator.GenerateRecursive(map, n: size + 1, top: size, bottom: 0, left: 0, right: size);

            Generator.Smooth(map);

            var result = new HeightMap(map);

            return result;
        }

        public Vector3[] FillVerticesAndNormalsAsTriangles()
        {
            int MapSize = map.GetLength(0);
            float cellSize = 1f;

            var normalsTemp = new List<Vector3>(6 * MapSize * MapSize);
            List<Vector3> result = new List<Vector3>(6 * MapSize * MapSize);

            var currentTriangle = new Vector3[3];
            var normalsForTriangle = new Vector3[3];

            for (int i = 0; i < MapSize; i++)
            {
                for (int j = 0; j < MapSize; j++)
                {
                    Vector3 v = new Vector3(i * cellSize, map[i, j], j * cellSize);
                    result.Add(v);
                    currentTriangle[0] = v;
                    if (i < MapSize - 1)
                    {
                        v = new Vector3((i + 1) * cellSize, map[i + 1, j], j * cellSize);
                    }
                    result.Add(v);
                    currentTriangle[1] = v;


                    if (j < MapSize - 1)
                    {
                        v = new Vector3(i * cellSize, map[i, j + 1], (j + 1) * cellSize);
                    }
                    result.Add(v);
                    currentTriangle[2] = v;

                    MathHelperMINE.CalcNormal(normalsForTriangle, currentTriangle);
                    for (int k = 0; k < normalsForTriangle.Length; k++)
                    {
                        normalsTemp.Add(normalsForTriangle[k]);
                    }
                    v = new Vector3(i * cellSize, map[i, j], j * cellSize);
                    result.Add(v);
                    currentTriangle[0] = v;


                    if (j < MapSize - 1)
                    {
                        v = new Vector3(i * cellSize, map[i, j + 1], (j + 1) * cellSize);
                    }
                    result.Add(v);
                    currentTriangle[1] = v;


                    if (i > 0 && j < MapSize - 1)
                    {
                        v = new Vector3((i - 1) * cellSize, map[i - 1, j + 1], (j + 1) * cellSize);
                    }
                    result.Add(v);
                    currentTriangle[2] = v;
                    MathHelperMINE.CalcNormal(normalsForTriangle, currentTriangle);
                    for (int k = 0; k < normalsForTriangle.Length; k++)
                    {
                        normalsTemp.Add(normalsForTriangle[k]);
                    }
                }
            }
            result.TrimExcess();
            normals = normalsTemp.ToArray();

            colors = GetColors(PrimitiveType.Triangles);

            return result.ToArray();
        }

        private Vector3[] FillVerticesAndNormalsAsLines()
        {
            int MapSize = map.GetLength(0);
            float cellSize = 1f;

            var result = new List<Vector3>(4 * MapSize * MapSize);

            for (int i = 0; i < MapSize; i++)
            {
                for (int j = 0; j < MapSize; j++)
                {
                    Vector3 v = new Vector3(i * cellSize, map[i, j], j * cellSize);

                    if (i < MapSize - 1)
                    {
                        var vNextX = new Vector3((i + 1) * cellSize, map[i + 1, j], j * cellSize);
                        result.Add(v);
                        result.Add(vNextX);
                    }
                    

                    if (j < MapSize - 1)
                    {
                        var vNextZ = new Vector3(i * cellSize, map[i, j + 1], (j + 1) * cellSize);
                        result.Add(v);
                        result.Add(vNextZ);
                    }
                }
            }
            result.TrimExcess();
            normals = new Vector3[] { new Vector3(0, 0 ,0) };

            colors = GetColors(PrimitiveType.Lines);

            return result.ToArray();
        }

        public Vector3[] GetColors(PrimitiveType mode)
        {
            if (mode == PrimitiveType.Triangles)
            {
                var result = new Vector3[6 * MapSize * MapSize];
                int k = 0;
                for (int i = 0; i < MapSize; i++)
                {
                    for (int j = 0; j < MapSize; j++)
                    {
                        Vector3 v = Colors[i, j];

                        result[k] = v;
                        k++;
                        result[k] = v;
                        k++;
                        result[k] = v;
                        k++;
                        result[k] = v;
                        k++;
                        result[k] = v;
                        k++;
                        result[k] = v;
                    }
                }

                return result;
            }
            if (mode == PrimitiveType.Lines)
            {
                var result = new Vector3[4 * MapSize * MapSize];
                int k = 0;
                for (int i = 0; i < MapSize; i++)
                {
                    for (int j = 0; j < MapSize; j++)
                    {
                        Vector3 v = Colors[i, j];

                        result[k] = v;
                        k++;
                        result[k] = v;
                        k++;
                        result[k] = v;
                        k++;
                        result[k] = v;
                    }
                }

                return result;
            }

            return null;
        }

        public SimpleModel GetAsModel()
        {
            if (points == null)
            {
                points = FillVerticesAndNormalsAsTriangles();
            }

            var model = new SimpleModel()
            {
                Vertices = points,
                Normals = normals,
                Colors = colors
            };

            return model;
        }
    }
}
