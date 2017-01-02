using OpenTK;

namespace ShadowMap
{
    public class LightCube : Cube
    {
        public LightCube(Vector3 center, Vector3 color, float scale = 1) : base(center, color, scale)
        {
            Center = center;
        }

        public Vector3 Center { get; set; }
        public Vector3 Target { get; set; }


        public void Tick()
        {
            var rot = Matrix4.CreateRotationY(MathHelper.PiOver6 * 0.1f);
            var tr = Matrix4.CreateTranslation(Target - Center);


            var v =Vector3.Transform(Center, rot);
            var v1 = Vector3.Transform(v, tr);

            Center = v1;
        }
    }
}
