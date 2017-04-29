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
using Common.Geometry;
using OcTreeLibrary;

namespace SimpleShooter
{
    class Engine
    {
        private KeyHandler _keyHandler;
        public GraphicsSystem _graphics;

        private List<GameObjectDescriptor> _gameObjects;
        private IPlayer _player;
        public Level _level;
        private OcTree _tree;

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
            InitTree();
        }

        private void InitTree()
        {
            _tree = new OcTree(_level.Volume);
            foreach (var o in _gameObjects)
            {
                if (o.RenderIdentity.ShaderKind != ShadersNeeded.Line)
                {
                    o.GameIdentity.OctreeItem.NeedReinsert += OctreeItem_NeedReinsert;
                    _tree.Insert(o.GameIdentity.OctreeItem);
                }
            }
        }

        private void OctreeItem_NeedReinsert(object sender, ReinsertingEventArgs args)
        {
            var gameObj = sender as IOctreeItem;
            if (gameObj == null)
                throw new ArgumentException();
            _tree.Remove(gameObj);
            gameObj.UpdateBoundingBox(args.NewBox);
            _tree.Insert(gameObj);
        }

        private void InitPlayer()
        {
            _player = _level.Player;
            _player.Jump += Player_Jump;
            _player.Move += Player_Move;
            _player.Shot += Player_Shot;
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
                var movable = obj.GameIdentity as IMovableObject;
                if (movable != null)
                {
                    movable.Move(delta);
                }
            }
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

        private void KeyPress(InputSignal signal)
        {
            _player.Handle(signal);
        }

    }
}
