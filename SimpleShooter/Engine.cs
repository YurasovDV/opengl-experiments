using System;
using System.Linq;
using System.Collections.Generic;
using Common.Input;
using OpenTK;
using SimpleShooter.Core;
using SimpleShooter.Graphics;
using SimpleShooter.PlayerControl;
using SimpleShooter.LevelLoaders;
using OcTreeLibrary;
using Common;
using System.Windows.Forms;
using SimpleShooter.Physics;
using SimpleShooter.Core.Enemies;
using SimpleShooter.Audio;

namespace SimpleShooter
{
    class Engine
    {
        private KeyHandler _keyHandler;
        public GraphicsSystem _graphics;

        private List<InputSignal> _eventsQueue;
        private List<GameObject> _nextGeneration;

        private IShooterPlayer _player;
        private ShootingController _playerCtrl;

        public Level _level;
        private OcTree _tree;

        private ObjectsGrouped _objects;

        public SoundManager SoundManager { get; set; }

        public Engine(int width, int height, IObjectInitializer initFunc, SoundManager sound)
        {
            InitObjects(initFunc);
            _graphics = new GraphicsSystem(width, height);

            _keyHandler = new KeyHandler();
            _keyHandler.KeysToWatch.Add(Keys.Space);
            _keyHandler.KeyPress += KeyPress;

            _eventsQueue = new List<InputSignal>();
            _nextGeneration = new List<GameObject>();

            SoundManager = sound;
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
            _nextGeneration.Add(projectile);
        }

        private void InitObjects(IObjectInitializer initFunc)
        {
            _objects = new ObjectsGrouped();

            _level = initFunc.CreateLevel();
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
            _playerCtrl = new ShootingController(this);
            _player.Shot += _playerCtrl.Player_Shot;

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
                KeyPress(_eventsQueue[i]);
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

            CheckCollisions(_player, delta);
        }

        private void AddNextGeneration()
        {
            foreach (var obj in _nextGeneration)
            {
                AddObject(obj);
            }

            _nextGeneration.Clear();
        }

        private void CheckCollisions(List<GameObjectDescriptor> entities, long delta)
        {
            foreach (var item in entities)
            {
                CheckCollisions(item.GameIdentity, delta);
            }
        }

        private void CheckCollisions(IOctreeItem entity, long delta)
        {
            var objectsToCheck = _tree.GetPossibleCollisions(entity);

            for (int i = 0; i < objectsToCheck.Count; i++)
            {
                if (Object.ReferenceEquals(entity,objectsToCheck[i]))
                {
                    continue;
                }

                if (entity.BoundingBox.Intersects(objectsToCheck[i].BoundingBox))
                {
                    if (entity is IMovableObject)
                    {
                        if (objectsToCheck[i] is IMovableObject)
                        {
                            HandleCollision(entity as IMovableObject, objectsToCheck[i] as IMovableObject);
                        }
                        else
                        {
                            HandleCollision(entity as IMovableObject, objectsToCheck[i]);
                        }
                    }
                    else
                    {
                        if (objectsToCheck[i] is IMovableObject)
                        {
                            HandleCollision(entity, objectsToCheck[i] as IMovableObject);
                        }
                        else
                        {
                            HandleCollision(entity, objectsToCheck[i]);
                        }
                    }
                }
            }
        }

        private void HandleCollision(IMovableObject obj1, IMovableObject obj2)
        {
            obj1.MoveAfterCollision(Vector3.Zero);
            obj2.MoveAfterCollision(Vector3.Zero);
        }

        private void HandleCollision(IMovableObject obj1, IOctreeItem obj2)
        {
            obj1.MoveAfterCollision(Vector3.Zero);
            // obj2.MoveAfterCollision(Vector3.Zero);
        }

        private void HandleCollision(IOctreeItem obj1, IMovableObject obj2)
        {
            //obj1.MoveAfterCollision(Vector3.Zero);
            //obj1.BoundingBox.
            obj2.MoveAfterCollision(Vector3.Zero);
        }

        private void HandleCollision(IOctreeItem obj1, IOctreeItem obj2)
        {
            //obj1.
            //obj1.MoveAfterCollision(Vector3.Zero);
            //obj2.MoveAfterCollision(Vector3.Zero);
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
                enemy.Shot += _playerCtrl.Enemy_Shot;
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

        private void KeyPress(InputSignal signal)
        {
            _player.Handle(signal);
        }

    }
}
