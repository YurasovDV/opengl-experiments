using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Utils;

namespace SimpleTerrain
{
    class Generator
    {
        private static Random random = new Random();

        private const float VARIATION = 0.05f;

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
        public static void GenerateRecursive(float[,] heightMap, int n, int bottom, int top, int left, int right)
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


        public static void Smooth(float[,] heightMap)
        {
            float maxDifference = 0;
            float difference = 0;
            float old = 0;
            // TODO: delete
            var iters = 0;
            do
            {
                maxDifference = 0;
                for (int i = 1; i < heightMap.GetLength(0) - 1; i++)
                {
                    for (int j = 1; j < heightMap.GetLength(1) - 1; j++)
                    {
                        old = heightMap[i, j];

                        heightMap[i, j] = old * 0.2f +
                            (float)MathHelperMINE.Average(heightMap[i - 1, j], heightMap[i, j - 1], heightMap[i + 1, j], heightMap[i, j + 1]) * 0.8f;

                        difference = (float)MathHelperMINE.Max(
                            heightMap[i - 1, j] - heightMap[i, j],
                            heightMap[i, j - 1] - heightMap[i, j],
                            heightMap[i + 1, j] - heightMap[i, j],
                            heightMap[i, j + 1] - heightMap[i, j]);

                        if (difference > maxDifference)
                        {
                            maxDifference = difference;
                        }
                    }
                }
                iters++;
            } while (maxDifference > 0.9f && iters < 5);
        }
    }
}
