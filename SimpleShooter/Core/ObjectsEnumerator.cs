using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleShooter.Graphics;

namespace SimpleShooter.Core
{
    internal class ObjectsEnumerator : IEnumerator<GameObjectDescriptor>
    {
        protected ShadersNeeded _objectsList;
        protected int? _positionInList;
        protected ObjectsGrouped _container;

        public ObjectsEnumerator(ObjectsGrouped container)
        {
            _container = container;
            Reset();
        }

        public GameObjectDescriptor Current
        {
            get
            {
                switch (_objectsList)
                {
                    case ShadersNeeded.SimpleModel:
                        return _container.GameObjectsSimpleModel.ElementAtOrDefault(_positionInList.Value);

                    case ShadersNeeded.Line:
                        return _container.GameObjectsLine.ElementAtOrDefault(_positionInList.Value);

                    case ShadersNeeded.TextureLess:
                        return _container.GameObjectsTextureLess.ElementAtOrDefault(_positionInList.Value);

                    case ShadersNeeded.TextureLessNoLight:
                        return _container.GameObjectsTextureLessNoLight.ElementAtOrDefault(_positionInList.Value);

                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public bool MoveNext()
        {
            bool res = false;
            if (_positionInList != null)
            {
                var list = _container.GetList(_objectsList);
                if (list != null && list.Count > _positionInList + 1)
                {
                    _positionInList++;
                    res = true;
                }
                else
                {
                    res = TryMoveToNextList();
                }
            }
            else
            {
                res = TryMoveToNextList();
            }

            return res;
        }

        private bool TryMoveToNextList()
        {
            bool res = false;
            _objectsList = ShadersOrder.GetNextShaderKind(_objectsList);
            while (_objectsList != ShadersNeeded.SkyBox)
            {
                var list = _container.GetList(_objectsList);
                if (list != null && list.Count > 0)
                {
                    res = true;
                    _positionInList = 0;
                    break;
                }
                _objectsList = ShadersOrder.GetNextShaderKind(_objectsList);
            }

            return res;
        }

        public void Reset()
        {
            _objectsList = ShadersNeeded.SkyBox;
            _positionInList = null;
        }

        public void Dispose()
        {

        }

        class ShadersOrder
        {
            public static ShadersNeeded GetNextShaderKind(ShadersNeeded current)
            {
                switch (current)
                {
                    case ShadersNeeded.SkyBox:
                        return ShadersNeeded.Line;

                    case ShadersNeeded.Line:
                        return ShadersNeeded.TextureLessNoLight;

                    case ShadersNeeded.TextureLessNoLight:
                        return ShadersNeeded.TextureLess;

                    case ShadersNeeded.TextureLess:
                        return ShadersNeeded.SimpleModel;

                    case ShadersNeeded.SimpleModel:
                        return ShadersNeeded.SkyBox;
                }
                throw new InvalidOperationException();
            }
        }
    }
}
