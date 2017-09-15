using System;
using System.Collections;
using System.Collections.Generic;
using SimpleShooter.Graphics;
using System.Linq;
using SimpleShooter.Core;

namespace SimpleShooter
{
    public class ObjectsGrouped : IEnumerable<GameObjectDescriptor>
    {
        public List<GameObjectDescriptor> GameObjectsSimpleModel { get; }
        public List<GameObjectDescriptor> GameObjectsLine { get; }
        public List<GameObjectDescriptor> GameObjectsTextureLess { get; }
        public List<GameObjectDescriptor> GameObjectsTextureLessNoLight { get; }

        public ObjectsGrouped()
        {
            GameObjectsSimpleModel = new List<GameObjectDescriptor>();
            GameObjectsLine = new List<GameObjectDescriptor>();
            GameObjectsTextureLess = new List<GameObjectDescriptor>();
            GameObjectsTextureLessNoLight = new List<GameObjectDescriptor>();
        }

        internal void AddObject(GameObjectDescriptor desc)
        {
            List<GameObjectDescriptor> listToInsert = GetList(desc);
            listToInsert.Add(desc);
        }

        internal void Remove(GameObjectDescriptor desc)
        {
            List<GameObjectDescriptor> listToInsert = GetList(desc);
            listToInsert.Remove(desc);
        }

        public List<GameObjectDescriptor> GetList(GameObjectDescriptor desc)
        {
            var shadersKind = desc.ShaderKind;
            var result = GetList(shadersKind);
            return result;
        }

        public List<GameObjectDescriptor> GetList(ShadersNeeded shadersKind)
        {
            List<GameObjectDescriptor> listToInsert = null;
            switch (shadersKind)
            {
                case ShadersNeeded.SimpleModel:
                    listToInsert = GameObjectsSimpleModel;
                    break;
                case ShadersNeeded.Line:
                    listToInsert = GameObjectsLine;
                    break;
                case ShadersNeeded.TextureLess:
                    listToInsert = GameObjectsTextureLess;
                    break;
                case ShadersNeeded.TextureLessNoLight:
                    listToInsert = GameObjectsTextureLessNoLight;
                    break;
                default:
                    break;
            }

            return listToInsert;
        }

        public IEnumerator<GameObjectDescriptor> GetEnumerator()
        {
            return new ObjectsEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new ObjectsEnumerator(this);
        }
    }
}
