using System;
using System.Collections.Generic;
using System.Linq;
using Common.Utils;
using OpenTK;

namespace ShadowMap
{
    public class Cube : GameObject
    {
        public Cube(Vector3 center, Vector3 color, float scale = 1)
        {
            var points = new List<Vector3>(VerticesForCube);
            for (int i = 0; i < points.Count; i++)
            {
                points[i] = (points[i] * scale) + center;
            }

            var colors = Enumerable.Repeat(color, points.Count).ToArray();

            var normals = GetNormals(points);

            Vertices = points.ToArray();
            Colors = colors;
            Normals = normals;


        }

        private Vector3[] GetNormals(List<Vector3> points)
        {
            var up = Vector3.UnitY;
            var normals = new Vector3[points.Count];
            var tempVertices = new Vector3[3];

            for (int i = 0; i < points.Count; i++)
            {
                int k = i / 6;
                Vector3 norm = up;

                if (k == 0)
                {
                    tempVertices[0] = points[0];
                    tempVertices[1] = points[1];
                    tempVertices[2] = points[2];

                    norm = CalcNormal(tempVertices);
                }


                if (k == 1)
                {
                    tempVertices[0] = points[6];
                    tempVertices[1] = points[7];
                    tempVertices[2] = points[8];

                    norm = CalcNormal(tempVertices);

                }
                if (k == 2)
                {
                    tempVertices[0] = points[12];
                    tempVertices[1] = points[13];
                    tempVertices[2] = points[14];

                    norm = CalcNormal(tempVertices);
                }
                if (k == 3)
                {
                    tempVertices[0] = points[18];
                    tempVertices[1] = points[19];
                    tempVertices[2] = points[20];

                    norm = CalcNormal(tempVertices);

                }

                if (k == 4)
                {
                    tempVertices[0] = points[24];
                    tempVertices[1] = points[25];
                    tempVertices[2] = points[26];

                    norm = CalcNormal(tempVertices);
                }

                if (k == 5)
                {
                    tempVertices[0] = points[30];
                    tempVertices[1] = points[31];
                    tempVertices[2] = points[32];

                    norm = CalcNormal(tempVertices);
                }

                normals[i] = norm;
            }

            return normals;
        }

        private const float Size = 1f;

        private static Vector3[] VerticesForCube = {

       new Vector3(-Size,  Size, -Size),
       new Vector3(-Size, -Size, -Size),
       new Vector3(Size, -Size, -Size),
       new Vector3(Size, -Size, -Size),
       new Vector3(Size,  Size, -Size),
       new Vector3(-Size,  Size, -Size),

        new Vector3(-Size, -Size,  Size),
        new Vector3(-Size, -Size, -Size),
        new Vector3(-Size,  Size, -Size),
        new Vector3(-Size,  Size, -Size),
        new Vector3(-Size,  Size,  Size),
        new Vector3(-Size, -Size,  Size),

         new Vector3(Size, -Size, -Size),
         new Vector3(Size, -Size,  Size),
         new Vector3(Size,  Size,  Size),
         new Vector3(Size,  Size,  Size),
         new Vector3(Size,  Size, -Size),
         new Vector3(Size, -Size, -Size),

        new Vector3(-Size, -Size,  Size),
        new Vector3(-Size,  Size,  Size),
        new Vector3( Size,  Size,  Size),
        new Vector3( Size,  Size,  Size),
        new Vector3( Size, -Size,  Size),
        new Vector3(-Size, -Size,  Size),

                //up
        new Vector3(-Size,  Size, -Size),
        new Vector3( Size,  Size, -Size),
        new Vector3( Size,  Size,  Size),
        new Vector3( Size,  Size,  Size),
        new Vector3(-Size,  Size,  Size),
        new Vector3(-Size,  Size, -Size),


                // down
        new Vector3(-Size, -Size, -Size),
        new Vector3(-Size, -Size,  Size),
        new Vector3( Size, -Size, -Size),
        new Vector3( Size, -Size, -Size),
        new Vector3(-Size, -Size,  Size),
        new Vector3( Size, -Size,  Size)
    };
    }
}
