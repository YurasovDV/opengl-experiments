namespace DeferredRender.Graphics.FrameBuffer
{
    public class FrameBufferDesc
    {
        public int PositionTextureId { get; set; }
        public int FramBufferObject { get; set; }
        //public int RenderBufferObject { get; set; }

        public int MainDepthMapBufferObject { get; set; }
        public int MainRenderBufferObject { get; set; }

        public int AttrVertexFrameLocation { get; set; }
        public int AttrTexcoordFrameLocation { get; set; }
        public int ColorAndSpectacularTextureId { get; internal set; }
        public int NormalTextureId { get; internal set; }
        public int DepthTextureId { get; internal set; }
    }
}
