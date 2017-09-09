namespace SimpleShooter.Graphics
{
    public class ShaderProgramDescriptor
    {
        public int ProgramId;

        public int AttribVerticesLocation;
        public int AttribColorsLocation;
        public int AttribNormalsLocation;
        public int AttribTextureCoordsLocation;


        public int verticesBuffer;
        public int colorsBuffer;
        public int normalsBuffer;
        public int textureCoordsBuffer;


        public int uniformLightPos;
        public int uniformMVP;
        public int uniformMV;
        public int uniformProjection;

        /// <summary>
        /// skybox
        /// </summary>
        public int TextureSampler;

        public ShadersNeeded ShaderKind { get; set; }

        public ShaderProgramDescriptor()
        {
            int notSet = -2;

            ProgramId = notSet;

            AttribVerticesLocation = notSet;
            AttribColorsLocation = notSet;
            AttribNormalsLocation = notSet;
            AttribTextureCoordsLocation = notSet;

            verticesBuffer = notSet;
            colorsBuffer = notSet;
            normalsBuffer = notSet;
            textureCoordsBuffer = notSet;


            uniformLightPos = notSet;
            uniformMVP = notSet;
            uniformMV = notSet;
            uniformProjection = notSet;

            TextureSampler = notSet;
        }

    }
}