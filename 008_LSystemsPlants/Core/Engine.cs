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

        private GeneratorSettings Settings { get; set; }
        private IGrammar Grammar { get; set; }

        public Engine(int width, int height, IGrammar grammar, GeneratorSettings settings)
        {
            _renderEngine = new RenderEngine(width, height);

            if (settings == null)
            {
                settings = GeneratorSettings.Default;
            }

            if (grammar == null)
            {
                grammar = new SimplestGrammar();
            }

            InitModel(grammar, settings);
        }

        public SimpleModel InitModel(IGrammar grammar, GeneratorSettings settings)
        {
            Settings = settings;

            var generator = new ModelGenerator();
            SimpleModel treeDefault = generator.Generate(grammar, Settings);
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
