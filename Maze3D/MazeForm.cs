using Maze3D.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

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

            using (StreamReader rd = new StreamReader("66dc48ca-95dc-11e5-8121-54271e1fe558.xml"))
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
