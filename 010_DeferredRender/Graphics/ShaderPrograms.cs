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
    /// reads all info, colors and positions from G-Buffer, and calculate lighting accordingly
    /// draw one quad(texture) and call it a day
    /// </summary>
    class FullScreenQuadProgram
    {
        public int ProgramId = 0;
        public int AttribVerticesLocation = 0;
        public int TexCoordsLocation = 0;

        public int verticesBuffer = 0;
        public int texCoordsBuffer = 0;

        public int uniformTextureColor = 0;

        public int uniformTextureDiffuse = 0;
    }

    class OneQuadProgram
    {
        public int ProgramId = 0;
        public int AttribVerticesLocation = 0;
        public int TexCoordsLocation = 0;

        public int verticesBuffer = 0;
        public int texCoordsBuffer = 0;

        public int uniformTexture0 = 0;

        /// <summary>
        /// flag isDepth: depth buffer needs to be linearized and differs drastically from the rest
        /// </summary>
        public int IsDepth = 0;
    }

    class LightVolumeProgram
    {
        public int ProgramId = 0;

        public int uniformMVP = 0;
        public int uniformMV = 0;
        internal int uniformTransform;


        public int uniformTexturePos = 0;
        public int uniformTextureNormal = 0;
        public int uniformTextureColor = 0;
        // mb is not needed
        public int uniformTextureDepth = 0;


        public int uniformColor = 0;
        public int uniformPosition = 0;


        public int AttribVerticesLocation = 0;

        public int verticesBuffer = 0;

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