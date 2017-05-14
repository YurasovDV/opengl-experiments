using System;
using System.Collections.Generic;
using Common.Utils;
using OpenTK;


namespace Common.Geometry
{
    public partial class BoundingVolume
    {
        public void MoveBox(Vector3 path)
        {
            VerticesBottom.TranslateAll(path);
            VerticesTop.TranslateAll(path);

            BottomLeftBack += path;
            TopRightFront += path;
            Centre += path;
        }
    }
}