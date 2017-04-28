using System.Collections.Generic;

namespace SimpleShooter.Graphics.ShaderLoader
{
    public class ShaderLoader
    {
        private static Dictionary<ShadersNeeded, IShaderLoader> _loaders = new Dictionary<ShadersNeeded, IShaderLoader>()
        {
            {ShadersNeeded.Line,  new ShaderLoaderLine()},
            {ShadersNeeded.SimpleModel, new ShaderLoaderSimpleModel() },
            {ShadersNeeded.TextureLess, new ShaderLoaderTextureLess() },
            {ShadersNeeded.TextureLessNoLight, new ShaderLoaderTextureLessNoLight() },
        };

        public static Dictionary<ShadersNeeded, int> programs = new Dictionary<ShadersNeeded, int>();

        public static bool TryGet(ShadersNeeded kind, out int programId)
        {
            if (programs.ContainsKey(kind))
            {
                programId = programs[kind];
                return true;
            }

            var desc = Load(kind);
            programId = desc.ProgramId;
            return true;
        }

        public static ShaderProgramDescriptor Load(ShadersNeeded kind)
        {
            var loader = _loaders[kind];
            var result = loader.Load();
            programs[kind] = result.ProgramId;

            return result;
        }

    }
}