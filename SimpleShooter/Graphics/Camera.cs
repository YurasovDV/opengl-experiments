using System.Collections.Generic;
using OpenTK;
using SimpleShooter.Graphics;
using SimpleShooter.Player;

namespace SimpleShooter.Graphics
{
    public class Camera
    {
        public Matrix4 Projection;
        public Matrix4 ModelView;
        public Matrix4 ModelViewProjection;

        public void RebuildMatrices(IShooterPlayer player)
        {
             ModelView = Matrix4.LookAt(player.Position, player.Target, Vector3.UnitY);
             ModelViewProjection = Matrix4.Mult(ModelView, Projection);
        }

        public Camera(Matrix4 projection)
        {
            Projection = projection;
        }
    }
}