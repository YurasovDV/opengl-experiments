using Common;
using DeferredRender.Graphics;
using OpenTK;

namespace DeferredRender
{
    public static class Extensions
    {
        public static void Bind(this SimpleModel model, Matrix4 modelView, Matrix4 modelViewProjection, Matrix4 projection)
        {
            if (model.TextureId == -1)
            {
                Shaders.BindTexturelessNoLight(model, modelView, modelViewProjection, projection);
            }
            else
            {
                Shaders.BindTexturedNoLight(model, modelView, modelViewProjection, projection);
            }
        }
    }
}
