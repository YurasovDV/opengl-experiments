using System.Collections.Generic;
using Common;
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

            InitModel();
        }

        private SimpleModel InitModel()
        {
            var generator = new ModelGenerator();
            SimpleModel treeDefault = generator.Generate(new SimplestGrammar());
            _trees = new List<SimpleModel>()
            {
                treeDefault
            };

            return treeDefault;
        }

        public void Tick(double delta)
        {
            _renderEngine.Begin();
            foreach (var tree in _trees)
            {
                _renderEngine.Render(tree, delta);
            }
            _renderEngine.End();
        }
    }
}
