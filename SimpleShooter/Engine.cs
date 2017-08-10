using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Common.Input;
using OcTreeLibrary;
using OpenTK;
using SimpleShooter.Audio;
using SimpleShooter.Core;
using SimpleShooter.Core.Enemies;
using SimpleShooter.Graphics;
using SimpleShooter.Physics;
using SimpleShooter.PlayerControl;

namespace SimpleShooter
{
    class Engine
    {
        private KeyHandler _keyHandler;
        private List<InputSignal> _eventsQueue;

        private Level _level;
        private TreeWrapper _treeWrapper;
        private ObjectsGrouped _objects;
        private List<GameObject> _nextObjectGeneration;

        private IShooterPlayer _player;
        private ShootingController _shootingCtrl;

        private GraphicsSystem _graphics;

        public ISoundManager SoundManager { get; set; }

        public Engine(int width, int height, Level level, ISoundManager sound)
        {
            _shootingCtrl = new ShootingController(this);

            _graphics = new GraphicsSystem(width, height);

            _keyHandler = new KeyHandler();
            _keyHandler.KeysToWatch.Add(Keys.Space);
            _keyHandler.KeyPress += OnKeyPress;

            _eventsQueue = new List<InputSignal>();
            _nextObjectGeneration = new List<GameObject>();

            SoundManager = sound;

            _objects = new ObjectsGrouped();

            _level = level;

            InitObjects();
        }

        internal void Dispose()
        {
            SoundManager.Dispose();
        }

        public void AddObjectAfterTick(GameObject projectile)
        {
            _nextObjectGeneration.Add(projectile);
        }

        private void InitObjects()
        {
            _treeWrapper = new TreeWrapper(_level);

            InitPlayer();

            foreach (var obj in _level.Objects)
            {
                AddObject(obj);
            }

            _level.Objects.Clear();
        }

        private void InitPlayer()
        {
            _player = _level.Player;
            _player.Shot += _shootingCtrl.Player_Shot;

            
            _treeWrapper.Insert(_player);
            _player.NeedsRemoval += _treeWrapper.OctreeItem_Remove;
            _player.NeedsInsert += _treeWrapper.OctreeItem_Insert;

        }

        public void Tick(long delta, Vector2 dxdy)
        {
            HandleAllInputs(dxdy);

            PhysicsStep(delta);

            CheckCollisions(delta);

            _graphics.Render(_objects, _level);

            AddNextGeneration();
        }

        #region input

        private void HandleAllInputs(Vector2 dxdy)
        {
            _keyHandler.CheckKeys();
            _player.HandleMouseMove(dxdy);

            for (int i = 0; i < _eventsQueue.Count; i++)
            {
                OnKeyPress(_eventsQueue[i]);
            }
            _eventsQueue.Clear();
        }

        public void PostEvent(InputSignal signal)
        {
            _eventsQueue.Add(signal);
        }

        private void OnKeyPress(InputSignal signal)
        {
            _player.Handle(signal);
        }

        #endregion

        private void PhysicsStep(long delta)
        {

            TickForMovable(_objects.GameObjectsLine, delta);
            TickForMovable(_objects.GameObjectsSimpleModel, delta);
            TickForMovable(_objects.GameObjectsTextureLess, delta);
            TickForMovable(_objects.GameObjectsTextureLessNoLight, delta);
             _player.Acceleration += AccelerationUpdater.GetGravityAcceleration(_player);
            _player.Tick(delta);
        }

        private static void TickForMovable(List<GameObjectDescriptor> listOfObjects, long delta)
        {
            foreach (var obj in listOfObjects)
            {
                var movable = obj.GameIdentity as IMovableObject;
                if (movable != null)
                {
                    movable.Acceleration += AccelerationUpdater.GetGravityAcceleration(movable);
                    movable.Tick(delta);
                }
            }
        }

        #region collisions

        private void CheckCollisions(long delta)
        {
            CheckCollisions(_objects.GameObjectsLine, delta);
            CheckCollisions(_objects.GameObjectsSimpleModel, delta);
            CheckCollisions(_objects.GameObjectsTextureLess, delta);
            CheckCollisions(_objects.GameObjectsTextureLessNoLight, delta);

            CheckCollisions((GameObject)_player, delta);
        }

        private void CheckCollisions(List<GameObjectDescriptor> entities, long delta)
        {
            foreach (var item in entities)
            {
                CheckCollisions(item.GameIdentity, delta);
            }
        }

        private void CheckCollisions(GameObject entity, long delta)
        {
            var objectsToCheck = _treeWrapper.GetPossibleCollisions(entity);

            for (int i = 0; i < objectsToCheck.Count; i++)
            {
                if (ReferenceEquals(entity, objectsToCheck[i]))
                {
                    continue;
                }

                Collisions.CheckAndHandle(entity, objectsToCheck[i]);
            }
        }

        #endregion

        public void AddObject(GameObject obj)
        {
            var desc = new GameObjectDescriptor(obj);
            _objects.AddObject(desc);

            var enemy = obj as Enemy;
            if (enemy != null)
            {
                enemy.Shot += _shootingCtrl.Enemy_Shot;
            }

            if (desc.RenderIdentity.ShaderKind != ShadersNeeded.Line)
            {
                desc.GameIdentity.NeedsRemoval += _treeWrapper.OctreeItem_Remove;
                desc.GameIdentity.NeedsInsert += _treeWrapper.OctreeItem_Insert;

                var volume = _treeWrapper.Insert(desc.GameIdentity);
                if (volume == null)
                {
                    _objects.Remove(desc);
                }
            }
        }

        private void AddNextGeneration()
        {
            foreach (var obj in _nextObjectGeneration)
            {
                AddObject(obj);
            }

            _nextObjectGeneration.Clear();
        }
    }
}
