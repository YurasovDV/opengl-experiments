using System.Collections.Generic;
using SimpleShooter.Graphics;

namespace SimpleShooter
{
    class ObjectsGrouped
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

        private List<GameObjectDescriptor> GetList(GameObjectDescriptor desc)
        {
            List<GameObjectDescriptor> listToInsert = null;
            switch (desc.ShaderKind)
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

    }
}
