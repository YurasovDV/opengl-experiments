using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace SimpleTerrain
{
    class RenderEngine : AbstractRenderEngine
    {
        public RenderEngine(int width, int height, AbstractPlayer player, float zFar = 200) 
            : base(width, height, player, zFar)
        {

        }
    }
}
