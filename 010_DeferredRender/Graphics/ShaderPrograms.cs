namespace DeferredRender.Graphics
{
    class TexturelessNoLight
    {
        public int uniformMVP = 0;
        public int uniformMV = 0;
        public int uniformProjection = 0;
        public int ProgramId = 0;
        public int AttribVerticesLocation = 0;
        public int AttribNormalsLocation = 0;
        public int AttribColorsLocation = 0;
        public int verticesBuffer = 0;
        public int colorsBuffer = 0;
        public int normalsBuffer = 0;
    }

    class TexturedNoLight
    {
        public int uniformMVP = 0;
        public int uniformMV = 0;
        public int uniformProjection = 0;
        public int ProgramId = 0;
        public int AttribVerticesLocation = 0;
        public int AttribNormalsLocation = 0;
        public int AttribColorsLocation = 0;
        public int TexCoordsLocation = 0;
        public int verticesBuffer = 0;
        public int colorsBuffer = 0;
        public int normalsBuffer = 0;
        public int texCoordsBuffer = 0;
        public int uniformTexture1 = 0;
    }

    /// <summary>
    /// reads all info, colors and positions from F-Buffer, and calculate lighting accordingly
    /// draw one quad(texture) and call it a day
    /// </summary>
    class GBufferSecondPass
    {
        public int ProgramId = 0;
        public int AttribVerticesLocation = 0;
        public int TexCoordsLocation = 0;

        public int verticesBuffer = 0;
        public int texCoordsBuffer = 0;

        public int uniformTexturePos = 0;
        public int uniformTextureNormal = 0;
        public int uniformTextureColor = 0;

        //public int UniformCameraPosition { get; set; }

    }

    class OneQuadProgram
    {
        public int ProgramId = 0;
        public int AttribVerticesLocation = 0;
        public int TexCoordsLocation = 0;

        public int verticesBuffer = 0;
        public int texCoordsBuffer = 0;

        public int uniformTexture0 = 0;
    }


    /// <summary>
    /// sending geometry only, without lighting
    /// </summary>
    class GBufferFirstPass
    {
        public int uniformMVP = 0;
        public int uniformMV = 0;
        public int uniformProjection = 0;
        public int ProgramId = 0;
        public int AttribVerticesLocation = 0;
        public int AttribNormalsLocation = 0;
        public int AttribColorsLocation = 0;
        public int TexCoordsLocation = 0;
        public int verticesBuffer = 0;
        public int colorsBuffer = 0;
        public int normalsBuffer = 0;
        public int texCoordsBuffer = 0;
        public int uniformTexture1 = 0;
    }


}