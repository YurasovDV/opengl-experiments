using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using LSystemsPlants.Core;
using LSystemsPlants.Core.Graphics;
using LSystemsPlants.Core.L_Systems;

namespace LSystemsPlants
{
    public class Engine
    {
        private List<SimpleModel> _trees;

        private RenderEngine _renderEngine;

        public Engine(int width, int height)
        {
            _renderEngine = new RenderEngine(width, height);

            var treeDefault = new ModelGenerator().Generate();

            _trees = new List<SimpleModel>()
            {
                treeDefault
            };
        }

        public void Tick(double delta)
        {
            _renderEngine.Render(delta);
        }
    }
}
