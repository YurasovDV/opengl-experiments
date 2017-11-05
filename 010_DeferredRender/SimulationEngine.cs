using System.Diagnostics;
using System.Windows.Forms;
using Common.Input;
using DeferredRender.Graphics;
using OpenTK;

namespace DeferredRender
{
    class SimulationEngine
    {
        private Stopwatch Watch { get; set; }

        private KeyHandler _keyHandler;
        private Player _player;
        private GraphicsSystem _graphics;

        public SimulationEngine(int width, int height, Stopwatch watch)
        {
            Watch = watch;

            _graphics = new GraphicsSystem(width, height);

            _player = new Player();

            _keyHandler = new KeyHandler();
            _keyHandler.KeysToWatch.Add(Keys.Space);
            _keyHandler.KeyPress += OnKeyPress;

            InitObjects();

        }

        private void InitObjects()
        {
            
        }


        private void OnKeyPress(InputSignal signal)
        {
            _player.Handle(signal);
        }

        internal void Tick(long timeSlice, Vector2 dxdy)
        {
            
        }
    }
}
