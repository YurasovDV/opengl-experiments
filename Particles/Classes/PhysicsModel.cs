using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using Particles.Graphics;
using Common;

namespace Particles.Classes
{
    public class PhysicsModel
    {
        private Random rand;
        private Particle[] particles = null;

        private Dictionary<int, Vector3> colorsDict = new Dictionary<int, Vector3>()
        {
            
            //{0, new Vector3(0.5f, 0, 0.5f)},
            //{1, new Vector3(1, 0, 0)},
            //{2, new Vector3(0, 0, 1)},
              

             {0, new Vector3(0.5f, 0, 0.5f)},
            {1, new Vector3(0.5f, 0, 0)},
            {2, new Vector3(0, 0, 0.5f)},

              
           /* {0, new Vector3(0, 0, 1)},
            {1, new Vector3(0, 0, 1)},
            {2, new Vector3(0, 0, 1)},*/
        };

        public bool IsFirstDraw { get; set; }

        /// <summary>
        /// размер частицы
        /// </summary>
        public const float PARTICLE_SIZE = 0.5f;

        public const int PATRICLE_COUNT = 300;

        public RenderEngine renderEngine { get; set; }

        public PhysicsModel(int size = PATRICLE_COUNT)
        {
            particles = new Particle[size];
            rand = new Random();
            for (int i = 0; i < size; i++)
            {
                var p = new Particle();
                ResetParticle(p);
                particles[i] = p;
            }
            IsFirstDraw = true;
        }

        private void ResetParticle(Particle p)
        {
            p.Place = new Vector3(0, 0, 1);
            var y = (float)rand.NextDouble() + 0.1f;
            var x = (float)rand.NextDouble() * 0.25f;
            if (rand.NextDouble() < 0.5)
            {
                x = -x;
            }
            p.Speed = new Vector3(x, y, 0);
        }

        public void Tick()
        {
            for (int i = 0; i < particles.Length; i++)
            {
                var p = particles[i];
                p.Place.X += p.Speed.X;
                p.Place.Y += p.Speed.Y;

                p.Speed.Y -= 0.03f;
                if (p.Place.Y < 0)
                {
                    ResetParticle(p);
                }
                if (Math.Abs(p.Speed.Y) < 0.03)
                {
                    ResetParticle(p);
                }
            }
            var points = GetPointsForRender();
            renderEngine.Vertices = points;
            renderEngine.Colors = GetColors(points);
            if (IsFirstDraw)
            {
                IsFirstDraw = false;
                
                renderEngine.TextureCoordinates = new Particles.Graphics.TextureManager().GetTextureCoordinates(points);
            }
            renderEngine.Draw();
        }

        private Vector3[] GetColors(Vector3[] pointsForRender)
        {
            var result = new Vector3[pointsForRender.Length];
            int currColorIndex = 0;
            for (int i = 0; i < result.Length - 3; i += 4)
            {
                var color = colorsDict[currColorIndex];
                for (int j = 0; j < 4; j++)
                {
                    var y = particles[i / 4].Speed.Y;
                    var c = new Vector3(color);
                    c.Z = y / 0.8f;
                    result[i + j] = c;
                }
                currColorIndex = (currColorIndex + 1) % 3;
            }
            return result;
        }

        public Vector3[] GetPointsForRender()
        {
            List<Vector3> res = new List<Vector3>(particles.Length * 6);
            Vector3 v;
            for (int i = 0; i < particles.Length; i++)
            {

                v = new Vector3(particles[i].Place);
                v.X += PARTICLE_SIZE;
                v.Y -= PARTICLE_SIZE;
                res.Add(v);


                v = new Vector3(particles[i].Place);
                v.X += PARTICLE_SIZE;
                v.Y += PARTICLE_SIZE;
                res.Add(v);

                v = new Vector3(particles[i].Place);
                v.X -= PARTICLE_SIZE;
                v.Y += PARTICLE_SIZE;
                res.Add(v);


                v = new Vector3(particles[i].Place);
                v.X -= PARTICLE_SIZE;
                v.Y -= PARTICLE_SIZE;
                res.Add(v);
            }
            return res.ToArray();
        }
    }
}
