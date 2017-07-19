using System.Linq;
using Common;
using Common.Utils;
using OpenTK;
using SimpleShooter.Core;
using SimpleShooter.Core.Enemies;
using SimpleShooter.Graphics;
using SimpleShooter.PlayerControl;

namespace SimpleShooter.Helpers
{
    class ProjectilesHelper
    {
        public static GameObject CreateProjectile(IShooterPlayer player)
        {
            var point = player.Position;
            var speed = Vector3.Normalize(player.Target - point) * 40;

            return CreateProjectile(player.Position, speed);
        }

        public static GameObject CreateProjectile(Enemy enemy)
        {
            var point = enemy.BoundingBox.Centre;
            var speed = Vector3.UnitX;

            return CreateProjectile(point, speed);
        }

        private static GameObject CreateProjectile(Vector3 barrelPosition, Vector3 speed)
        {
            var model = new SimpleModel();
            model.Vertices = GeometryHelper.GetVerticesForCube(0.05f);

            for (int i = 0; i < model.Vertices.Length; i++)
            {
                model.Vertices[i] += barrelPosition;
            }

            model.Colors = Enumerable.Repeat(new Vector3(1, 0, 0), model.Vertices.Length).ToArray();
            var movableObj = new MovableObject(model, ShadersNeeded.TextureLessNoLight, speed, Vector3.Zero);
            movableObj.CalcNormals();
            return movableObj;
        }
    }
}
