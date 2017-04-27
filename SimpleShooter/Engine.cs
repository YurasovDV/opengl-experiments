using System;
using System.Collections.Generic;
using Common.Input;
using OpenTK;
using SimpleShooter.Core;

namespace SimpleShooter
{
    class Engine
    {
        private GraphicsSystem _graphics;
        private List<GameObject> _models;
        private KeyHandler _keyHandler;
        private PlayerModel PlayerModel;

        public Engine(GraphicsSystem graphics, IObjectInitialiser initFunc)
        {
            _graphics = graphics;
            _models = new List<GameObject>();
            InitObjects();
            _keyHandler = new KeyHandler();
            _keyHandler.KeyPress += KeyPress;
            PlayerModel = new PlayerModel();
        }

        private void InitObjects()
        {
            var objects = new ObjectInitializer().CreateLevel();
            _models.AddRange(objects);
        }

        internal void Tick(long delta, Vector2 dxdy)
        {
            _keyHandler.CheckKeys();
            PlayerModel.Handle(dxdy, _graphics.Camera);
            _graphics.Render(_models);
        }

        private void KeyPress(InputSignal signal)
        {
            PlayerModel.Handle(signal, _graphics.Camera);
        }

    }
}
