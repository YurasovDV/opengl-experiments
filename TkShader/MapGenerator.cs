using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShaderOnForm
{
    public class MapGenerator
    {
        private static Random random = new Random();
        
        static MapGenerator()
        {
        }

        public static HeightMap Generate(int size)
        {
            if (size % 2 != 0)
            {
                throw new ArgumentException("MapGenerator.Generate: размер карты не делится на 2");
            }
            if (size < 64)
            {
                throw new ArgumentException("MapGenerator.Generate: размер карты меньше 64");
            }

            double[,] heightMap = new double[size + 1, size + 1];
            GenerateRecursive(heightMap, n: size + 1, top: size, bottom: 0, left: 0, right: size);
            Smooth(heightMap);

            return new HeightMap(heightMap);
        }

        private static void Smooth(double[,] heightMap)
        {
            double maxDifference = 0;
            double difference = 0;
            double old = 0;
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

                        heightMap[i, j] = old * 0.2 +
                            MathHelperMINE.Average(heightMap[i - 1, j], heightMap[i, j - 1], heightMap[i + 1, j], heightMap[i, j + 1]) * 0.8;

                        difference = MathHelperMINE.Max(
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
            } while (maxDifference > Constants.MAX_DIFFERENCE && iters < 5);
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
        private static void GenerateRecursive(double[,] heightMap, int n, int bottom, int top, int left, int right)
        {
            var halfSize = n / 2; //n >> 1;
            if (halfSize < 1) return;
            bool addOrSubstract = random.NextDouble() <= 0.5;
            heightMap[bottom + halfSize, left + halfSize] =
                MathHelperMINE.Average
                (
                heightMap[bottom, left],
                heightMap[bottom, right],
                heightMap[top, left],
                heightMap[top, right]
                ) + (addOrSubstract ? halfSize : -halfSize) * Constants.VARIATION;

            addOrSubstract = random.NextDouble() <= 0.5;
            heightMap[bottom + halfSize, left] =
                (heightMap[bottom, left] + heightMap[top, left]) * 0.5
                + (addOrSubstract ? halfSize : -halfSize) * Constants.VARIATION;

            addOrSubstract = random.NextDouble() <= 0.5;
            heightMap[bottom + halfSize, right] = (heightMap[bottom, right] + heightMap[top, right]) * 0.5
                + (addOrSubstract ? halfSize : -halfSize) * Constants.VARIATION;

            addOrSubstract = random.NextDouble() <= 0.5;
            heightMap[bottom, left + halfSize] = (heightMap[bottom, left] + heightMap[bottom, right]) * 0.5
                + (addOrSubstract ? halfSize : -halfSize) * Constants.VARIATION;

            addOrSubstract = random.NextDouble() <= 0.5;
            heightMap[top, left + halfSize] = (heightMap[top, left] + heightMap[top, right]) * 0.5
                + (addOrSubstract ? halfSize : -halfSize) * Constants.VARIATION;

            // 1
            GenerateRecursive(heightMap, halfSize, bottom: bottom, top: bottom + halfSize, left: left, right: left + halfSize);
            // 2
            GenerateRecursive(heightMap, halfSize, bottom: bottom, top: bottom + halfSize, left: left + halfSize, right: right);
            // 3
            GenerateRecursive(heightMap, halfSize, bottom: bottom + halfSize, top: top, left: left, right: left + halfSize);
            // 4
            GenerateRecursive(heightMap, halfSize, bottom: bottom + halfSize, top: top, left: left + halfSize, right: right);

        }
    }
}
