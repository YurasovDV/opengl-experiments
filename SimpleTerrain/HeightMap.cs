using System;
using System.Collections.Generic;
using Common;
using Common.Utils;
using OpenTK;

namespace SimpleTerrain
{
    class HeightMap
    {
        public int MapSize { get { return Colors.GetLength(0); } }

        private float[,] map;
        private Vector3[,] Colors { get; set; }


        private Vector3[] points;
        private Vector3[] normals;
        private Vector3[] colors;

        private static Random random = new Random();

        private const float VARIATION = 0.05f;

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

            colors = GetColors();
        }

        public static HeightMap Create(int size)
        {

            float[,] map = new float[size + 1, size + 1];

            GenerateRecursive(map, n: size + 1, top: size, bottom: 0, left: 0, right: size);

            var result = new HeightMap(map);



            return result;
        }

        /// <summary>
        /// |-------- bottom
        /// |                 
        /// |left                 right
        /// |                
        /// |----------top
        /// </summary>
        /// <param name="heightMap"></param>
        /// <param name="n"></param>
        /// <param name="bottom"></param>
        /// <param name="top"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        private static void GenerateRecursive(float[,] heightMap, int n, int bottom, int top, int left, int right)
        {
            var halfSize = n / 2; //n >> 1;

            if (halfSize < 1)
                return;
            bool addOrSubstract = random.NextDouble() <= 0.5;

            heightMap[bottom + halfSize, left + halfSize] =
                (heightMap[bottom, left] +
                heightMap[bottom, right] +
                heightMap[top, left] +
                heightMap[top, right]) * 0.25f + VARIATION * halfSize * (addOrSubstract ? 1 : -1);

            addOrSubstract = random.NextDouble() <= 0.5;

            heightMap[bottom + halfSize, left] =
                (heightMap[bottom, left] + heightMap[top, left]) * 0.5f
                + VARIATION * halfSize * (addOrSubstract ? 1 : -1);

            addOrSubstract = random.NextDouble() <= 0.5;

            heightMap[bottom + halfSize, right] = (heightMap[bottom, right] + heightMap[top, right]) * 0.5f
                + VARIATION * halfSize * (addOrSubstract ? 1 : -1);

            addOrSubstract = random.NextDouble() <= 0.5;

            heightMap[bottom, left + halfSize] = (heightMap[bottom, left] + heightMap[bottom, right]) * 0.5f
                + VARIATION * halfSize * (addOrSubstract ? 1 : -1);

            addOrSubstract = random.NextDouble() <= 0.5;

            heightMap[top, left + halfSize] = (heightMap[top, left] + heightMap[top, right]) * 0.5f
                + VARIATION * halfSize * (addOrSubstract ? 1 : -1);

            // 1
            GenerateRecursive(heightMap, halfSize, bottom: bottom, top: bottom + halfSize, left: left, right: left + halfSize);
            // 2
            GenerateRecursive(heightMap, halfSize, bottom: bottom, top: bottom + halfSize, left: left + halfSize, right: right);
            // 3
            GenerateRecursive(heightMap, halfSize, bottom: bottom + halfSize, top: top, left: left, right: left + halfSize);
            // 4
            GenerateRecursive(heightMap, halfSize, bottom: bottom + halfSize, top: top, left: left + halfSize, right: right);

        }

        public Vector3[] GetPoints(out Vector3[] normals)
        {
            int MapSize = map.GetLength(0);
            float CellSize = 0.7f;

            var normalsTemp = new List<Vector3>(6 * MapSize * MapSize);
            List<Vector3> result = new List<Vector3>(6 * MapSize * MapSize);

            var currentTriangle = new Vector3[3];
            var normalsForTriangle = new Vector3[3];

            for (int i = 0; i < MapSize; i++)
            {
                for (int j = 0; j < MapSize; j++)
                {
                    Vector3 v = new Vector3(i * CellSize, map[i, j], j * CellSize);
                    result.Add(v);
                    currentTriangle[0] = v;
                    if (i < MapSize - 1)
                    {
                        v = new Vector3((i + 1) * CellSize, map[i + 1, j], j * CellSize);
                    }

                    result.Add(v);
                    currentTriangle[1] = v;

                    if (j < MapSize - 1)
                    {
                        v = new Vector3(i * CellSize, map[i, j + 1], (j + 1) * CellSize);
                    }
                    result.Add(v);
                    currentTriangle[2] = v;

                    MathHelperMINE.CalcNormal(normalsForTriangle, currentTriangle);
                    for (int k = 0; k < normalsForTriangle.Length; k++)
                    {
                        normalsTemp.Add(normalsForTriangle[k]);
                    }


                    v = new Vector3(i * CellSize, map[i, j], j * CellSize);
                    result.Add(v);
                    currentTriangle[0] = v;
                    if (j < MapSize - 1)
                    {
                        v = new Vector3(i * CellSize, map[i, j + 1], (j + 1) * CellSize);
                    }
                    result.Add(v);
                    currentTriangle[1] = v;
                    if (i > 0 && j < MapSize - 1)
                    {
                        v = new Vector3((i - 1) * CellSize, map[i - 1, j + 1], (j + 1) * CellSize);
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
            return result.ToArray();
        }

        public Vector3[] GetColors()
        {
            var result = new Vector3[6 * MapSize * MapSize];

            int k = 0;
            for (int i = 0; i < MapSize; i++)
            {
                for (int j = 0; j < MapSize; j++)
                {
                    Vector3 v = Colors[i, j];

                    result[k] = (v);
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

        public SimpleModel GetAsModel()
        {
            if (points == null)
            {
                points = GetPoints(out normals);
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
