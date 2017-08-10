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
        public GraphicsSystem _graphics;

        private List<InputSignal> _eventsQueue;
        private List<GameObject> _nextObjectGeneration;

        private IShooterPlayer _player;
        private ShootingController _shootingCtrl;

        public Level _level;
        private OcTree _tree;

        private ObjectsGrouped _objects;

        public SoundManager SoundManager { get; set; }

        public Engine(int width, int height, Level level)
        {
            _shootingCtrl = new ShootingController(this);

            _graphics = new GraphicsSystem(width, height);

            _keyHandler = new KeyHandler();
            _keyHandler.KeysToWatch.Add(Keys.Space);
            _keyHandler.KeyPress += OnKeyPress;

            _eventsQueue = new List<InputSignal>();
            _nextObjectGeneration = new List<GameObject>();

            SoundManager = new SoundManager();

            _objects = new ObjectsGrouped();

            _level = level;

            InitObjects();
        }

        internal void PostEvent(InputSignal signal)
        {
            _eventsQueue.Add(signal);
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
            _tree = new OcTree(_level.Volume);

            InitPlayer();

            foreach (var obj in _level.Objects)
            {
                AddObject(obj);
            }

            _level.Objects.Clear();
        }

        private void OctreeItem_Remove(object sender, ReinsertingEventArgs args)
        {
            var gameObj = sender as IOctreeItem;
            if (gameObj == null)
                throw new ArgumentException();
            _tree.Remove(gameObj);
        }

        private void OctreeItem_Insert(object sender, ReinsertingEventArgs args)
        {
            var gameObj = sender as IOctreeItem;
            if (gameObj == null)
                throw new ArgumentException();
            var v = _tree.Insert(gameObj);
            if (v == null)
            {
                //_objects.Remove(gameObj);
            }
        }

        private void InitPlayer()
        {
            _player = _level.Player;
            _player.Shot += _shootingCtrl.Player_Shot;

            _tree.Insert(_player);
            _player.NeedsRemoval += OctreeItem_Remove;
            _player.NeedsInsert += OctreeItem_Insert;

        }

        internal void Tick(long delta, Vector2 dxdy)
        {
            HandleAllInputs(dxdy);

            PhysicsStep(delta);

            CheckCollisions(delta);

            _graphics.Render(_objects, _level);

            AddNextGeneration();
        }


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

        private void PhysicsStep(long delta)
        {

            TickForMovable(_objects.GameObjectsLine, delta);
            TickForMovable(_objects.GameObjectsSimpleModel, delta);
            TickForMovable(_objects.GameObjectsTextureLess, delta);
            TickForMovable(_objects.GameObjectsTextureLessNoLight, delta);
             _player.Acceleration += AccelerationUpdater.GetGravityAcceleration(_player);
            _player.Tick(delta);
        }

        private void CheckCollisions(long delta)
        {
            CheckCollisions(_objects.GameObjectsLine, delta);
            CheckCollisions(_objects.GameObjectsSimpleModel, delta);
            CheckCollisions(_objects.GameObjectsTextureLess, delta);
            CheckCollisions(_objects.GameObjectsTextureLessNoLight, delta);

            CheckCollisions((GameObject)_player, delta);
        }

        private void AddNextGeneration()
        {
            foreach (var obj in _nextObjectGeneration)
            {
                AddObject(obj);
            }

            _nextObjectGeneration.Clear();
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
            var objectsToCheck = _tree.GetPossibleCollisions(entity);

            for (int i = 0; i < objectsToCheck.Count; i++)
            {
                if (object.ReferenceEquals(entity, objectsToCheck[i]))
                {
                    continue;
                }

                Collisions.CheckAndHandle(entity, objectsToCheck[i]);
            }
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
                desc.GameIdentity.NeedsRemoval += OctreeItem_Remove;
                desc.GameIdentity.NeedsInsert += OctreeItem_Insert;

                var volume = _tree.Insert(desc.GameIdentity);
                if (volume == null)
                {
                    _objects.Remove(desc);
                }
            }
        }

        private void OnKeyPress(InputSignal signal)
        {
            _player.Handle(signal);
        }

    }
}
