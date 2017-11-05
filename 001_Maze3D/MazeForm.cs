using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;
using Maze3D.Classes;

namespace Maze3D
{
    public partial class MazeForm : Form
    {
        public object objLock;

        private Engine engine;

        public MazeForm()
        {
            InitializeComponent();
            objLock = new object();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            XmlSerializer sr = new XmlSerializer(typeof(Maze));

            SendOrPostCallback refreshCall = (state) =>
                {
                    lock (objLock)
                    {
                        RefreshScreen();
                    }
                };

            var syncContext = WindowsFormsSynchronizationContext.Current;

            Action refresh = () =>
                {
                    syncContext.Post(refreshCall, null);
                };

            engine = new Engine(refresh, portraitControl.Width, portraitControl.Height, portraitControl, objLock);

            using (StreamReader rd = new StreamReader("Maze.xml"))
            {
                engine.Maze = (Maze)sr.Deserialize(rd);
            }
        }

        private void MazeForm_Shown(object sender, EventArgs e)
        {
            portraitControl.Context.MakeCurrent(null);
            engine.Go();
        }



        private void RefreshScreen()
        {
            if (!(portraitControl.IsDisposed || portraitControl.Disposing))
            {
                /*if (!portraitControl.Context.IsCurrent)
                {
                    portraitControl.Context.MakeCurrent(null);
                    portraitControl.MakeCurrent();
                }*/
                try
                {
                    portraitControl.MakeCurrent();
                    portraitControl.SwapBuffers();
                    //portraitControl.Invalidate();
                    portraitControl.Context.MakeCurrent(null);
                }
                catch (Exception)
                {

                    System.Diagnostics.Debug.WriteLine("error at RefreshScreen()");
                }
                
            }
        }
    }
}
