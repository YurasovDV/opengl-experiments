using OpenTK;

namespace ShadowMap
{
    public  class LightSphere : GameObject
    {
        public LightSphere(Vector3 center)
        {
            Center = center;
        }

        public Vector3 Center { get; set; }
        public Vector3 Target { get; set; }


        public LightSphere Tick()
        {
            var rot = Matrix4.CreateRotationY(0.015f);
            var tr = Matrix4.CreateTranslation(-Target);
            var trBack = Matrix4.CreateTranslation(Target);
            var combined = tr * rot * trBack;

            var centerTransformed = Vector3.Transform(Center, combined);

            var temp = new LightSphere(centerTransformed)
            {
                Target = Target,
                Colors = Colors,
                Normals = Normals,
                TextureCoordinates = TextureCoordinates,
                TextureId = TextureId,
                Vertices = new Vector3[Vertices.Length]
            };

            for (int i = 0; i < Vertices.Length; i++)
            {
                temp.Vertices[i] = Vector3.Transform(Vertices[i], combined);
            }

            return temp;
        }
    }
}
