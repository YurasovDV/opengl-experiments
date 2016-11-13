using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using OpenTK.Graphics.OpenGL4;
using OpenTK;
using ShaderOnForm;


namespace TkShader
{
    public partial class Form1 : Form
    {


        private Engine engine;
        //private Task genMap;
        private Timer mainTimer;
        private KeyHandler keyHandler;

        //private HeightMap map = null;

        public Form1()
        {
            InitializeComponent();
        }

        public void Render()
        {
            portraitControl.SwapBuffers();
            //portraitControl.Refresh();
        }

        private void gameTick(object sender, EventArgs e)
        {
            engine.Tick();
            this.Text = String.Format("x={0:F4}       z={1:F4}    angle={2:F4}   eyex={3:F4}   eyez={4:F4}",
                engine.Vehicle.X,
                engine.Vehicle.Z,
                engine.Vehicle.AngleHorizontal
                , engine.Vehicle.EyeX
                , engine.Vehicle.EyeZ
                );


        }

        private void RenderFrame()
        {
            this.portraitControl.SwapBuffers();
            portraitControl.Refresh();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            GraphicsManager graph = new GraphicsManager(RenderFrame);
            engine = new Engine(graph);//new SimplePhysEngine(graph);
            graph.InitGraphics(portraitControl.Width, portraitControl.Height);
            var heightMap = engine.Map;
            //WriteToFile(heightMap);


            /* genMap = new Task(
                 () =>
                 {
                     GraphicsManager graph = new GraphicsManager(RenderFrame);
                     engine = new Engine(graph);//new SimplePhysEngine(graph);
                 });
             genMap.ContinueWith((task) =>
             {
                 var heightMap = engine.Map;
                 //WriteToFile(heightMap);
                 var context = WindowsFormsSynchronizationContext.Current;
                 if (context != null)
                 {
                     context.Send(new System.Threading.SendOrPostCallback(
                         (state) => { this.Text = state as string; }), "written");
                 }
             });

             genMap.Start();*/
            mainTimer = new Timer()
            {
                Interval = 1000 / Constants.PREFERRED_FPS
            };
            mainTimer.Tick += gameTick;

            AbstractVehicle vehicle = new Helicopter();
            engine.Vehicle = vehicle;
            keyHandler = new KeyHandler();
            keyHandler.OnKeyPress += engine.ProcessKey;
            engine.KeyHandler = keyHandler;
            mainTimer.Start();
        }
    }
}     
          
     
