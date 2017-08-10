using System.Collections.Generic;
using OpenTK;

namespace ShaderOnForm
{
    public class HeightMap
    {
        private double[,] heightMap;

        public HeightMap(double[,] heightMap)
        {
            this.heightMap = heightMap;
        }
        public double this[int i, int j]
        {
            get
            {
                return heightMap[i, j];
            }
            set
            {

            }
        }

        public Vector3[] GetPoints(out Vector3[] normals)
        {
            var normalsTemp = new List<Vector3>(4 * Constants.MAP_SIZE * Constants.MAP_SIZE);
            List<Vector3> result = new List<Vector3>(4 * Constants.MAP_SIZE * Constants.MAP_SIZE);
            var vrt = new List<Vector3>();
            for (int i = 0; i < Constants.MAP_SIZE; i++)
            {
                for (int j = 0; j < Constants.MAP_SIZE; j++)
                {
                    vrt.Clear();

                    Vector3 v = new Vector3(i * Constants.CELL_SIZE, (float)heightMap[i, j], j * Constants.CELL_SIZE);
                    result.Add(v);
                    vrt.Add(v);
                    if (i < Constants.MAP_SIZE - 1)
                    {
                        v = new Vector3((i + 1) * Constants.CELL_SIZE, (float)heightMap[i + 1, j], j * Constants.CELL_SIZE);
                    }

                    result.Add(v);
                    vrt.Add(v);

                    if (j < Constants.MAP_SIZE - 1)
                    {
                        v = new Vector3(i * Constants.CELL_SIZE, (float)heightMap[i, j + 1], (j + 1) * Constants.CELL_SIZE);
                    }
                    result.Add(v);
                    vrt.Add(v);

                   MathHelperMINE.CalcNormal(normalsTemp, vrt);

                    vrt.Clear();

                    //  /*
                    v = new Vector3(i * Constants.CELL_SIZE, (float)heightMap[i, j], j * Constants.CELL_SIZE);
                    result.Add(v);
                    vrt.Add(v);
                    if (j < Constants.MAP_SIZE - 1)
                    {
                        v = new Vector3(i * Constants.CELL_SIZE, (float)heightMap[i, j + 1], (j + 1) * Constants.CELL_SIZE);
                    }
                    result.Add(v);
                    vrt.Add(v);
                    if (i > 0 && j < Constants.MAP_SIZE - 1)
                    {
                        v = new Vector3((i - 1) * Constants.CELL_SIZE, (float)heightMap[i - 1, j + 1], (j + 1) * Constants.CELL_SIZE);
                    }

                    result.Add(v);
                    vrt.Add(v);
                    MathHelperMINE.CalcNormal(normalsTemp, vrt);
                }
            }
            result.TrimExcess();
            normals = normalsTemp.ToArray();
            return result.ToArray();
        }
    }
}
