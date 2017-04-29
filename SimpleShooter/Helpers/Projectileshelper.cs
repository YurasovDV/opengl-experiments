using System.Linq;
using Common;
using Common.Utils;
using OpenTK;
using SimpleShooter.Core;
using SimpleShooter.Graphics;
using SimpleShooter.Player;

namespace SimpleShooter.Helpers
{
    class Projectileshelper
    {
        public static GameObject CreateProjectile(IShooterPlayer player)
        {
            var point = player.Position;
            var speed = Vector3.Normalize(player.Target - point) * 2;

            var model = new SimpleModel();
            model.Vertices = GeometryHelper.GetVerticesForCube(0.2f);

            for (int i = 0; i < model.Vertices.Length; i++)
            {
                model.Vertices[i] += player.Position;
            }

            model.Colors = Enumerable.Repeat(new Vector3(1, 0, 0), model.Vertices.Length).ToArray();
            var movableObj = new MovableObject(model, ShadersNeeded.TextureLess, speed);
            movableObj.CalcNormals();
            return movableObj;
        }
    }
}
