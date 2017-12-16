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

    // draw one quad(texture) and call it a day
    class OneQuadProgram
    {
        public int ProgramId = 0;
        public int AttribVerticesLocation = 0;
        public int TexCoordsLocation = 0;
        public int verticesBuffer = 0;
        public int texCoordsBuffer = 0;
        public int uniformTexture1 = 0;
    }
}