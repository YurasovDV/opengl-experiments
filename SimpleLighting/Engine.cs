using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleLighting
{
    class Engine
    {
        GraphicSystem graphic;
        private OpenTK.GLControl portraitControl;
        private Action refresh;

        public Object LockObj { get; set; }

        public Engine(OpenTK.GLControl portraitControl, Action refresh, object objectForGLContextLock)
        {
            this.portraitControl = portraitControl;
            graphic = new GraphicSystem(portraitControl.Width, portraitControl.Height, refresh);
            LockObj = objectForGLContextLock;
            this.refresh = refresh;
        }

        public void Start()
        {
            Thread t = new Thread(MainCycle);
            t.IsBackground = true;
            t.Start();
        }

        public void MainCycle()
        {
            
            PhysModel model = new PhysModel();
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            var startTime = 0L; 

            while (true)
            {
                var newVal = watch.ElapsedMilliseconds;
                var delta = newVal - startTime;
                model.Tick(delta);
                var models = model.GetModelsForRender();
                lock (LockObj)
                {
                    try
                    {
                        // portraitControl.Context.MakeCurrent(null);
                        portraitControl.MakeCurrent();
                        graphic.Render(models);
                        portraitControl.Context.MakeCurrent(null);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("engine exit");
                        Debug.WriteLine(ex.Message);
                        //break;
                    }
                    refresh();
                }
                
                startTime = newVal;
                
                /*if (delta < 10)
                {
                    Thread.Sleep(20);
                }*/
            }
        }
    }
}
