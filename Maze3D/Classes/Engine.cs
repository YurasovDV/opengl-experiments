using Maze3D.Classes.Graphics;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Maze3D.Classes
{
    class Engine
    {
        public RenderEngine GraphEngine { get; set; }
        public Maze Maze { get; set; }

        public Player PlayerData { get; set; }

        public GLControl portraitControl { get; set; }
        public Action refresh { get; set; }

        public object LockObj { get; set; }

        public Engine(Action refresh, int width, int height, GLControl portraitControl, object lockObj)
        {
            PlayerData = new Player();
            GraphEngine = new RenderEngine(PlayerData, width, height);
            Maze = new Maze();
            LockObj = lockObj;
            this.portraitControl = portraitControl;
            this.refresh = refresh;

        }

        public void Go()
        {
            Thread t = new Thread(MainCycle);
            t.IsBackground = true;
            t.Start();
        }

        public void MainCycle()
        {
            Stopwatch watch = new Stopwatch();
            long startTime = 0;
            long newVal  = 0;
            watch.Start();

            while (true)
            {
                lock (LockObj)
                {
                    try
                    {
                        // portraitControl.Context.MakeCurrent(null);
                        portraitControl.MakeCurrent();
                        var look = PlayerData.Target - PlayerData.Position;
                        GraphEngine.SetMaze(Maze.GetAsModel(look));
                        newVal = watch.ElapsedMilliseconds;
                        GraphEngine.Render(newVal - startTime);
                        portraitControl.Context.MakeCurrent(null);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("engine fail");
                        Debug.WriteLine(ex.Message);
                    }
                    refresh();
                }

                if (newVal - startTime <= 10)
                {
                    Thread.Sleep(50);
                }

                startTime = newVal;
 
            }
        }
    }
}
