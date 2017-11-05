using System;
using System.Windows.Forms;

using Particles.Classes;
using Particles.Graphics;

namespace Particles
{
    public partial class ParticleForm : Form
    {
        private Timer mainTimer;
        public PhysicsModel Model { get; set; }
        public ParticleForm()
        {
            InitializeComponent();
        }

        private void ParticleForm_Load(object sender, EventArgs e)
        {
            Model = new PhysicsModel();
            RenderEngine graph = new RenderEngine(portraitControl.Width, portraitControl.Height);
            graph.InitGraphics();
            Model.renderEngine = graph;
            mainTimer = new Timer()
            {
                Interval = 1000 / 80
            };
            mainTimer.Tick += gameTick;


            mainTimer.Start();
        }

        private void gameTick(object sender, EventArgs e)
        {
            Model.Tick();
            portraitControl.SwapBuffers();
            portraitControl.Refresh();
        }

        private void ParticleForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ')
            {
                if (mainTimer.Enabled)
                {
                    mainTimer.Stop();
                }
                else
                {
                    mainTimer.Start();
                }
            }
        }
    }
}
