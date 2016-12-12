using System;
using System.Collections.Generic;
using Common;
using Common.Utils;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace ShadowMap
{
    class HeightMap
    {
        public int MapSize { get { return Colors.GetLength(0); } }

        public int TextureId { get; set; }
        public PrimitiveType ModelType { get; set; }

        private float[,] map;
        private Vector3[,] Colors { get; set; }


        private Vector3[] points;
        private Vector3[] normals;
        private Vector3[] colors;
        private Vector2[] textureCoords;

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

            ModelType = PrimitiveType.Triangles;
        }

        public static HeightMap Create(int size)
        {
            float[,] map = new float[size + 1, size + 1];
            Generator.GenerateFlat(map, -3);
            var result = new HeightMap(map);
            return result;
        }

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

        public void RebuildVerticesAccordingly(PrimitiveType renderMode)
        {
            points = GetPoints(renderMode);
            ModelType = renderMode;
        }

        private Vector3[] GetPoints(PrimitiveType renderMode)
        {
            if (renderMode == PrimitiveType.Triangles)
            {
                return FillModelAsTriangles();
            }
            if (renderMode == PrimitiveType.Lines)
            {
                return FillModelAsLines();
            }

            throw new ArgumentException("incorrect renderMode", "renderMode");
        }

        public Vector3[] FillModelAsTriangles()
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

                    if (i < MapSize - 1 && j < MapSize - 1)
                    {
                        result.Add(v);
                        currentTriangle[0] = v;

                        v = new Vector3((i + 1) * cellSize, map[i + 1, j], j * cellSize);
                        currentTriangle[1] = v;
                        result.Add(v);

                        v = new Vector3(i * cellSize, map[i, j + 1], (j + 1) * cellSize);
                        currentTriangle[2] = v;
                        result.Add(v);


                        GameObject.CalcNormal(currentTriangle, normalsForTriangle);
                        for (int k = 0; k < normalsForTriangle.Length; k++)
                        {
                            normalsTemp.Add(normalsForTriangle[k]);
                        }

                    }


                    if (j < MapSize - 1 && i > 0 && j < MapSize - 1)
                    {
                        v = new Vector3(i * cellSize, map[i, j], j * cellSize);
                        result.Add(v);
                        currentTriangle[0] = v;

                        v = new Vector3(i * cellSize, map[i, j + 1], (j + 1) * cellSize);
                        result.Add(v);
                        currentTriangle[1] = v;

                        v = new Vector3((i - 1) * cellSize, map[i - 1, j + 1], (j + 1) * cellSize);
                        result.Add(v);
                        currentTriangle[2] = v;
                        GameObject.CalcNormal(currentTriangle, normalsForTriangle);
                        for (int k = 0; k < normalsForTriangle.Length; k++)
                        {
                            normalsTemp.Add(normalsForTriangle[k]);
                        }

                    }
                }
            }
            result.TrimExcess();
            normals = normalsTemp.ToArray();

            colors = GetColors(PrimitiveType.Triangles);

            return result.ToArray();
        }

        private Vector3[] FillModelAsLines()
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
            normals = new Vector3[] { new Vector3(0, 0, 0) };

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

        private Vector3[] InitModelAsPolygons()
        {
            var triangles = FillModelAsTriangles();

            var textureMgr = new TextureManager();
            TextureId = textureMgr.LoadTexture(@"Assets\Textures\grass.jpg");

            textureCoords = textureMgr.GetTextureCoordinates(triangles);

            return triangles;
        }

        public SimpleModel GetAsModel()
        {
            if (points == null)
            {
                points = InitModelAsPolygons();
            }

            var model = new SimpleModel()
            {
                Vertices = points,
                Normals = normals,
                Colors = colors,
            };

            // System.Diagnostics.Debug.Assert(colors.Length == points.Length);

            if (ModelType == PrimitiveType.Triangles)
            {
                model.TextureCoordinates = textureCoords;
                model.TextureId = TextureId;
            }
            else
            {
                model.TextureId = -1;
            }
            return model;
        }
    }
}
