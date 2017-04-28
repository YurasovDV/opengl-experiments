using System;
using System.Linq;
using System.Collections.Generic;
using Common.Input;
using OpenTK;
using SimpleShooter.Core;
using SimpleShooter.Graphics;
using SimpleShooter.Player;
using SimpleShooter.LevelLoaders;

namespace SimpleShooter
{
    class Engine
    {
        private KeyHandler _keyHandler;
        public GraphicsSystem _graphics;

        private List<GameObjectDescriptor> _gameObjects;
        private PlayerModel _playerModel;
        public Level _level;

        public Engine(int width, int height, IObjectInitializer initFunc)
        {           
            InitObjects(initFunc);
            _graphics = new GraphicsSystem(width, height);
            _keyHandler = new KeyHandler();
            _keyHandler.KeyPress += KeyPress;

        }

        private void InitObjects(IObjectInitializer initFunc)
        {
            _gameObjects = new List<GameObjectDescriptor>();
            _level = initFunc.CreateLevel();
            _playerModel = _level.Player;
            _gameObjects.AddRange(_level.Objects.Select(o => new GameObjectDescriptor(o)));
        }

        internal void Tick(long delta, Vector2 dxdy)
        {
            _keyHandler.CheckKeys();
            _playerModel.Handle(dxdy);
            PhysicsStep(delta);
            _graphics.Render(_gameObjects, _level);
        }

        private void PhysicsStep(long delta)
        {
            foreach (var obj in _gameObjects)
            {
                var movable = obj.PhysicalObject as IMovableObject;
                if (movable != null)
                {
                    movable.Move(delta);
                }
            }
        }

        private void KeyPress(InputSignal signal)
        {
            _playerModel.Handle(signal);
        }

    }
}
