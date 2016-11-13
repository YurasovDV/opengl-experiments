using Common;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using oct = OcTreeExample.OcTree;
using System.IO;

namespace OcTreeExample
{
    class Engine : AbstractEngine
    {
        public Player Player { get; set; }

        public Particle[] particles { get; set; }

        public Random rand { get; set; }

        private SimpleModel cachedModel = null;

        public LineRender LineRend { get; set; }

        public oct.OcTree MainTree { get; set; }

        /// <summary>
        /// размер частицы
        /// </summary>
        public const float PARTICLE_SIZE = 0.5f;

        public const int PATRICLE_COUNT = 300;


        public Engine(int Width, int Height)
            : base(Width, Height)
        {
            Player = new Player();
            KeyHandler.KeyPress += Player.OnSignal;
            var renderEngine = new RenderEngine(Width, Height, Player);
            ParticleRenderer = renderEngine;
            CreateParticles();

            LineRend = new LineRender(Width, Height, Player, renderEngine);

            CreateTree();
        }

        private void CreateTree()
        {
            var centre = new Vector3(0, 0, 0);
            var halfSize = 80;

            var allVolume = oct.BoundingVolume.CreateVolume(centre, halfSize);

            MainTree = new oct.OcTree(allVolume);

            // прямо по курсу
            //var sampleCenter1 = new Vector3(27, 5, 30);
            //var sampleVolume1 = CreateVolume(sampleCenter1, 1);
            //MainTree.Insert(sampleVolume1);

            // в дальней плоскости
            //var sampleCenter = new Vector3(-10, 10, 10);
            //var sampleVolume = CreateVolume(sampleCenter, 1);
            //MainTree.Insert(sampleVolume);


            /*
            var sampleCenter3 = new Vector3(27, 24, 27);
            var sampleVolume3 = oct.BoundingVolume.CreateVolume(sampleCenter3, 1);
            MainTree.Insert(sampleVolume3);

            var sampleCenter2 = new Vector3(27, 28, 27);
            var sampleVolume2 = oct.BoundingVolume.CreateVolume(sampleCenter2, 1);
            MainTree.Insert(sampleVolume2);*/

            //List<oct.BoundingVolume> objects = ;
            CreateListObjects().ForEach(o => MainTree.Insert(o));

            
            MainTree.Serialize("C:\\Users\\1\\Desktop\\out.txt");

        }

        private List<oct.BoundingVolume> CreateListObjects()
        {
            var result = new List<oct.BoundingVolume>();

            var y = 20;

            var o = oct.BoundingVolume.CreateVolume(new Vector3(-5, y, 5), 10, 20);
            o.Name = "n8";
            result.Add(o);

            o = oct.BoundingVolume.CreateVolume(new Vector3(-47, y, 0), 5, 10);
            o.Name = "n7";
            result.Add(o);

            o = oct.BoundingVolume.CreateVolume(new Vector3(-10, y, 40), 15, 10);
            o.Name = "n10";
            result.Add(o);

            o = oct.BoundingVolume.CreateVolume(new Vector3(40, y, -5), 10, 10);
            o.Name = "n9";
            result.Add(o);

            o = oct.BoundingVolume.CreateVolume(new Vector3(10, y, -25), 15, 10);
            o.Name = "n6";
            result.Add(o);

            o = oct.BoundingVolume.CreateVolume(new Vector3(-2, y, -70), 10, 4);
            o.Name = "n2";
            result.Add(o);



            o = oct.BoundingVolume.CreateVolume(new Vector3(-40, y, -40), 20, 10);
            o.Name = "n1";
            result.Add(o);

            o = oct.BoundingVolume.CreateVolume(new Vector3(4.5f, y, -55), 4, 10);
            o.Name = "n3";
            result.Add(o);

            o = oct.BoundingVolume.CreateVolume(new Vector3(20f, y, -60), 8, 8);
            o.Name = "n4";
            result.Add(o);

            o = oct.BoundingVolume.CreateVolume(new Vector3(33f, y, -63), 3, 10);
            o.Name = "n5";
            result.Add(o);

            return result;
        }

        private void CreateParticles(int size = PATRICLE_COUNT)
        {
            particles = new Particle[size];
            rand = new Random();
            for (int i = 0; i < size; i++)
            {
                var p = new Particle();
                ResetParticle(p);
                particles[i] = p;
            }
        }


        public override void Click(Vector2 point)
        {
           // clicked = true;
        }

        public override void Tick(long timeSlice, Vector2 dxdy)
        {
            KeyHandler.CheckKeys();

            //Player.Tick(timeSlice, dxdy);

            UpdateParticles();

            SimpleModel model = GetParticlesAsModel();


            GL.ClearColor(0, 0f, 0.1f, 100);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);


            //ParticleRenderer.SetupVieport();


            var treeModel = MainTree.GetModel();


            //ParticleRenderer.Render(new SimpleModel[] { model });

            LineRend.Render(treeModel);

            GL.Flush();
        }

        private void UpdateParticles()
        {
            for (int i = 0; i < particles.Length; i++)
            {
                var p = particles[i];
                p.Place.X += p.Speed.X;
                p.Place.Y += p.Speed.Y;
                p.Place.Z += p.Speed.Z;

                p.Speed.Y -= 0.01f;
                if (p.Place.Y < 0)
                {
                    ResetParticle(p);
                }
                if (p.Speed.Y < 0.01)
                {
                    ResetParticle(p);
                }
            }
        }

        private SimpleModel GetParticlesAsModel()
        {
            InitCachedParticleModel();

            return cachedModel;
        }

        private void InitCachedParticleModel()
        {
            var model = new SimpleModel();
            float[] speedArray;
            model.Vertices = GetPointsForRender(out speedArray);

            Vector3 blue = new Vector3(0, 0, 0.5f);

            var colors = new Vector3[model.Vertices.Length]; // Enumerable.Repeat(blue, model.Vertices.Length).ToArray();

            for (int i = 0; i < speedArray.Length; i++)
            {
                colors[i] = new Vector3(0, 0, speedArray[i] * 1.5f);
            }

            model.Colors = colors;

            if (cachedModel == null)
            {
                model.Normals = new Vector3[1];
                model.TextureCoordinates = GetTextureCoordinates(model.Vertices);
                string path = @"Assets\Pics\particle3.png";
                model.TextureId = new TextureManager().LoadTexture(path);
            }
            else
            {
                model.TextureCoordinates = cachedModel.TextureCoordinates;
                model.TextureId = cachedModel.TextureId;
            }


            cachedModel = model;
        }

        private void ResetParticle(Particle p)
        {
            p.Place = new Vector3((float)(rand.NextDouble() * 0.5), (float)(rand.NextDouble() * 0.5), 0);
            var y = (float)rand.NextDouble() * 0.7f;
            var x = (float)rand.NextDouble() * 0.15f;
            var z = (float)rand.NextDouble() * 0.15f;
            if (rand.NextDouble() < 0.5)
            {
                x = -x;
            }
            if (rand.NextDouble() < 0.5)
            {
                z = -z;
            }
            p.Speed = new Vector3(x, y, z);
        }


        public Vector2[] GetTextureCoordinates(Array particles)
        {
            Vector2[] texCoordsPoints = new Vector2[]
            {
                new Vector2(1.0f, 0.0f),
                new Vector2(1.0f, 1.0f),
                new Vector2(0.0f, 1.0f),
                new Vector2(0.0f, 0.0f), 	
			   /* 
                new Vector2(1.0f, 0.0f),
		    	new Vector2(0.0f, 1.0f),*/
		    	
            };

            var texCoordsPointsCurrent = new Vector2[particles.Length];
            for (int i = 0; i < texCoordsPointsCurrent.Length; i++)
            {
                texCoordsPointsCurrent[i] = texCoordsPoints[i % 4];
            }

            return texCoordsPointsCurrent;
        }

        public Vector3[] GetPointsForRender(out float[] speeds)
        {
            /* var look = Player.Position - Player.Target;
             look = new Vector3(look.X, 0, look.Z);
             var angle = Vector3.CalculateAngle(look, Vector3.UnitZ);

             if (clicked)
             {
                 Debug.WriteLine("angle " + angle);
                 Debug.WriteLine("look " + look.X + "   " + look.Z);
                 clicked = false;
             }
             */

            Matrix4 rotMatrix = Matrix4.CreateRotationY(Player.AngleHorizontalRad);

            var vertices = new Vector3[particles.Length * 4];
            speeds = new float[particles.Length * 4];

            for (int i = 0, j = 0; i < particles.Length; i++, j += 4)
            {
                var v = new Vector3(particles[i].Place);

                var v1 = Vector3.Transform(new Vector3(PARTICLE_SIZE, -PARTICLE_SIZE, 0), rotMatrix);
                vertices[j] = v + v1;

                v1 = Vector3.Transform(new Vector3(PARTICLE_SIZE, PARTICLE_SIZE, 0), rotMatrix);
                vertices[j + 1] = v + v1;

                v1 = Vector3.Transform(new Vector3(-PARTICLE_SIZE, PARTICLE_SIZE, 0), rotMatrix);
                vertices[j + 2] = v + v1;

                v1 = Vector3.Transform(new Vector3(-PARTICLE_SIZE, -PARTICLE_SIZE, 0), rotMatrix);
                vertices[j + 3] = v + v1;

                speeds[j] = particles[i].Speed.Y;
                speeds[j + 1] = particles[i].Speed.Y;
                speeds[j + 2] = particles[i].Speed.Y;
                speeds[j + 3] = particles[i].Speed.Y;

            }
            return vertices;
        }

      //  public bool clicked { get; set; }
    }
}
