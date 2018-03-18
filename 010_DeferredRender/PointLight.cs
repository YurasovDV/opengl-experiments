using Common;
using OpenTK;

namespace DeferredRender
{
    class PointLight : SimpleModel
    {
        public float RotateAngleDefault { get; set; }

        public PointLight(float rotateAngleDefault)
        {
            RotateAngleDefault = rotateAngleDefault;
        }

        public Vector3 CurrentPosition { get; set; }

        public float Radius { get; set; }

        public Vector3 Color
        {
            get
            {
                if (Colors == null)
                {
                    return Vector3.Zero;
                }
                return Colors[0];
            }
        }

        public SimpleModel DenotationCube { get; set; }

        public Matrix4 Transform;

        internal void Tick(long timeSlice)
        {
            var rot = Matrix4.CreateRotationY(RotateAngleDefault  * timeSlice * 0.001f);
            var tr = Matrix4.CreateTranslation(CurrentPosition);
            var trBack = Matrix4.CreateTranslation(-CurrentPosition);
            var combined = tr * rot * trBack;

            CurrentPosition = Vector3.Transform(CurrentPosition, combined);


            var scale = Matrix4.CreateScale(Radius);
            var translateUpdated = Matrix4.CreateTranslation(CurrentPosition);
            Transform = scale * translateUpdated;
        }
    }
}
