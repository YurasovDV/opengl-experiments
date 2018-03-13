namespace DeferredRender.Graphics.FrameBuffer
{
    public class FrameBufferDesc
    {
        public int ColorAndSpectacularTextureId { get; set; }
        public int NormalTextureId { get; set; }
        public int DepthTextureId { get; set; }

        public int PositionTextureId { get; set; }


        public int DiffuseTextureId { get; set; }

        /// <summary>
        /// is not needed for now
        /// </summary>
        public int SpectacularTextureId { get; set; }

        public int FrameBufferObject { get; set; }
    }
}
