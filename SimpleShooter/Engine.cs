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
        private GraphicsSystem _graphics;
        private List<GameObjectDescriptor> _gameObjects;
        private KeyHandler _keyHandler;
        private PlayerModel _playerModel;

        public Engine(GraphicsSystem graphics, IObjectInitializer initFunc)
        {
            _graphics = graphics;
            
            InitObjects(initFunc);
            _keyHandler = new KeyHandler();
            _keyHandler.KeyPress += KeyPress;

        }

        private void InitObjects(IObjectInitializer initFunc)
        {
            _gameObjects = new List<GameObjectDescriptor>();
            var level = initFunc.CreateLevel();
            _playerModel = level.Player;
            _gameObjects.AddRange( level.Objects.Select(o => new GameObjectDescriptor(o)));
        }

        internal void Tick(long delta, Vector2 dxdy)
        {
            _keyHandler.CheckKeys();
            _playerModel.Handle(dxdy);
            PhysicsStep(delta);
            _graphics.Render(_gameObjects, _playerModel);
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
