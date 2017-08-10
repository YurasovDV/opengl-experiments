using System;
using OpenTK;
using SimpleShooter.Core;
using SimpleShooter.Graphics;
using SimpleShooter.LevelLoaders;

namespace GLTests
{
    class Utils
    {
        static Vector3 _green;
        static ShadersNeeded _shaders;

        static Utils()
        {
            _green = new Vector3(0, 1, 0);
            _shaders = ShadersNeeded.TextureLess;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="center"></param>
        /// <param name="size">actual cube will have dimension = size * 2</param>
        /// <returns></returns>
        public static GameObject CreateCube(Matrix4 center, float size)
        {
            return ObjectInitializer.CreateCube(center, _green, size, _shaders);
        }
    }
}
