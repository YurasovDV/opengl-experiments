using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL4;

namespace LSystemsPlants
{
    public partial class MainForm : Form
    {
        private Engine _engine;
        private Timer _timer;

        public MainForm()
        {
            InitializeComponent();

            
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            _engine.Tick(_timer.Interval);
            RenderFrame();
        }

        private void RenderFrame()
        {
            portraitControl.SwapBuffers();
            portraitControl.Refresh();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _engine = new Engine(portraitControl.Width, portraitControl.Height);

            _timer = new Timer()
            {
                Interval = 60
            };
            _timer.Tick += _timer_Tick;

            _timer.Start();
        }
    }
}
