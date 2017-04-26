using System.Collections.Generic;
using OpenTK;
using SimpleShooter.Graphics;

namespace SimpleShooter
{
    public class Camera
    {
        public Matrix4 Projection;
        public Matrix4 ModelView;
        public Matrix4 ModelViewProjection;


        public Vector3 Position { get; set; }
        public Vector3 Target { get; set; }
        public Vector3 LightPosition { get; set; }

        public void RebuildMatrices()
        {
             ModelView = Matrix4.LookAt(Position, Target, Vector3.UnitY);
             ModelViewProjection = Matrix4.Mult(ModelView, Projection);
        }

        public Camera(Matrix4 projection)
        {
            Projection = projection;
        }
    }
}