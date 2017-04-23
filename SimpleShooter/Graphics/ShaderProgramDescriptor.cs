namespace SimpleShooter.Graphics
{
    public class ShaderProgramDescriptor
    {
        public int  ProgramId;

        public ShadersNeeded ShadersKind;

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

        public ShaderProgramDescriptor()
        {

        }

    }
}