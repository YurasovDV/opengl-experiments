namespace Glass.Graphics
{
    public class FrameBufferDesc
    {
        public int ColorAndSpectacularTextureId { get; set; }
        public int NormalTextureId { get; set; }
        public int DepthTextureId { get; set; }

        public int PositionTextureId { get; set; }


        public int DiffuseTextureId { get; set; }

        public int FrameBufferObject { get; set; }
    }
}
