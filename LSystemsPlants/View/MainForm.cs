using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LSystemsPlants
{
    public partial class MainForm : Form
    {
        private Engine _engine;
        private Timer _timer;

        public MainForm()
        {
            InitializeComponent();

            _engine = new Engine();

            _timer = new Timer()
            {
                Interval = 16
            };
            _timer.Tick += _timer_Tick;
            _timer.Start();
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            _engine.Tick(_timer.Interval);
        }

        private void RenderFrame()
        {
            portraitControl.SwapBuffers();
            portraitControl.Refresh();
        }
    }
}
