using System.Collections.Generic;

namespace SimpleShooter.Graphics.ShaderLoad
{
    public class ShaderLoader
    {
        private static Dictionary<ShadersNeeded, IShaderLoader> _loaders = new Dictionary<ShadersNeeded, IShaderLoader>()
        {
            {ShadersNeeded.Line,  new ShaderLoaderLine()},
            {ShadersNeeded.SimpleModel, new ShaderLoaderSimpleModel() },
            {ShadersNeeded.TextureLess, new ShaderLoaderTextureLess() },
            {ShadersNeeded.TextureLessNoLight, new ShaderLoaderTextureLessNoLight() },
            {ShadersNeeded.SkyBox, new ShaderLoaderSkybox() },
        };

        public static Dictionary<ShadersNeeded, ShaderProgramDescriptor> programs = new Dictionary<ShadersNeeded, ShaderProgramDescriptor>();

        public static bool TryGet(ShadersNeeded kind, out ShaderProgramDescriptor programId)
        {
            if (programs.ContainsKey(kind))
            {
                programId = programs[kind];
                return true;
            }

            var desc = Load(kind);
            programId = desc;
            return true;
        }

        private static ShaderProgramDescriptor Load(ShadersNeeded kind)
        {
            var loader = _loaders[kind];
            var result = loader.Load();
            programs[kind] = result;
            return result;
        }

    }
}