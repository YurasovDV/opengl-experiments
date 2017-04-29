using System;
using System.Linq;
using System.Collections.Generic;
using Common.Input;
using OpenTK;
using SimpleShooter.Core;
using SimpleShooter.Graphics;
using SimpleShooter.Player;
using SimpleShooter.LevelLoaders;
using SimpleShooter.Player.Events;
using tree = OcTreeRevisited.OcTree;

namespace SimpleShooter
{
    class Engine
    {
        private KeyHandler _keyHandler;
        public GraphicsSystem _graphics;

        private List<GameObjectDescriptor> _gameObjects;
        private IPlayer _player;
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
            _gameObjects.AddRange(_level.Objects.Select(o => new GameObjectDescriptor(o)));
            InitPlayer();

           // tree.OcTree tree = new tree.OcTree();
           // tree.Insert(
        }

        private void InitPlayer()
        {
            _player = _level.Player;
            _player.Jump += Player_Jump;
            _player.Move += Player_Move;
            _player.Shot += Player_Shot;
        }

        private ActionStatus Player_Shot(object sender, ShotEventArgs args)
        {
            var res = new ActionStatus()
            {
                Success = true
            };

            return res;
        }

        private ActionStatus Player_Move(object sender, MoveEventArgs args)
        {
            var res = new ActionStatus()
            {
                Success = true
            };

            return res;
        }

        private ActionStatus Player_Jump(object sender, JumpEventArgs args)
        {
            var res = new ActionStatus()
            {
                Success = true
            };

            return res;
        }

        internal void Tick(long delta, Vector2 dxdy)
        {
            _keyHandler.CheckKeys();
            _player.HandleMouseMove(dxdy);
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
            _player.Handle(signal);
        }

    }
}
