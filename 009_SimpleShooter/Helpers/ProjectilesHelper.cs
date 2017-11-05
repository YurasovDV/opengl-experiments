using System;
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
            var speed = Vector3.Normalize(enemy.Target - point) * 40;

            return CreateProjectile(enemy.Target, speed);
        }

        private static GameObject CreateProjectile(Vector3 barrelPosition, Vector3 speed)
        {
            var angleBetwSpeedAndModel = Vector3.CalculateAngle(Vector3.UnitX, new Vector3(speed.X, 0, speed.Z));
            var rotation = Matrix4.CreateRotationY(angleBetwSpeedAndModel);

            var model = new SimpleModel(@"Content\Models\cone.obj", null);

            MakeSmaller10x(model);

            for (int i = 0; i < model.Vertices.Length; i++)
            {
                var v = model.Vertices[i];
                v = Vector3.Transform(v, rotation);
                v += barrelPosition;

                model.Vertices[i] = v;
            }

            model.Colors = Enumerable.Repeat(new Vector3(1, 0, 0), model.Vertices.Length).ToArray();
            var movableObj = new MovableObject(model, ShadersNeeded.TextureLess, speed, Vector3.Zero, 0.001f);
            return movableObj;
        }

        private static void MakeSmaller10x(SimpleModel model)
        {
            var scale = Matrix4.CreateScale(0.1f);
            for (int i = 0; i < model.Vertices.Length; i++)
            {
                model.Vertices[i] = Vector3.Transform(model.Vertices[i], scale);
            }
        }
    }
}
