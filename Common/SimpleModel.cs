using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using OpenTK;

namespace Common
{
    public class SimpleModel
    {
        public Vector3[] Vertices { get; set; }
        public Vector3[] Colors { get; set; }
        public Vector3[] Normals { get; set; }

        public Vector2[] TextureCoordinates { get; set; }

        private int textureId = -1;

        public int TextureId
        {
            get { return textureId; }
            set { textureId = value; }
        }

        public SimpleModel()
        {

        }

        public SimpleModel(string pathModel, string texturePath)
        {
            List<Vector3> verts = new List<Vector3>();
            List<Vector2> textureUV = new List<Vector2>();
            List<Vector3> normals = new List<Vector3>();


            List<Vector3> vertResult = new List<Vector3>();
            List<Vector2> textureResult = new List<Vector2>();
            List<Vector3> normalsResult = new List<Vector3>();


            using (StreamReader rd = new StreamReader(pathModel))
            {
                string row = null;
                while ((row = rd.ReadLine()) != null)
                {
                    var parts = row.Split(' ');
                    if (parts.Length < 3)
                    {
                        continue;
                    }

                    Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

                    var type = parts[0];

                    if (type == "v")
                    {
                        Vector3 v = new Vector3(float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3]));
                        verts.Add(v);
                    }
                    else if (type == "vt")
                    {
                        Vector2 v = new Vector2(float.Parse(parts[1]), float.Parse(parts[2]));
                        textureUV.Add(v);
                    }
                    else if (type == "vn")
                    {
                        Vector3 v = new Vector3(float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3]));
                        normals.Add(v);
                    }
                    else if (type == "f")
                    {
                        foreach (var vertexDesc in parts.Skip(1))
                        {
                            var data = vertexDesc.Split('/');

                            var vertexIndex = int.Parse(data[0]) - 1;
                            var textureIndex = !string.IsNullOrEmpty(data[1]) ? int.Parse(data[1]) - 1 : 1;
                            var normalIndex = int.Parse(data[2]) - 1;

                            vertResult.Add(verts[vertexIndex]);
                            if (textureUV.Count > textureIndex)
                            {
                                textureResult.Add(textureUV[textureIndex]);
                            }
                            normalsResult.Add(normals[normalIndex]);

                        }
                    }
                    else
                        continue;
                }
            }

            Vertices = vertResult.ToArray();
            Vector3 color = new Vector3(1, 0, 0);
            Colors = Enumerable.Repeat<Vector3>(color, vertResult.Count).ToArray();
            Normals = normalsResult.ToArray();
            TextureCoordinates = textureResult.ToArray();

            TextureManager mgr = new TextureManager();
            if (texturePath != null)
            {
                TextureId = mgr.LoadTexture(texturePath);
            }
            else
            {
                TextureId = -1;
            }
        }
    }
}
