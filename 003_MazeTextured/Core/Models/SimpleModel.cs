using OpenTK;

namespace SimpleShadows.Core.Models
{
    public class SimpleModel
    {
        public Vector3[] Vertices { get; set; }
        public Vector3[] Color { get; set; }
        public Vector3[] Normals { get; set; }

        public Vector2[] TextureCoordinates { get; set; }

        private int textureId = -1;

        public int TextureId
        {
            get { return textureId; }
            set { textureId = value; }
        }

    }
}
