#version 330

out vec4 outputColor;
uniform sampler2D uTexture;
uniform int isDepth;
in vec2 texCoords;



float linearizeDepth()
{
    float zNear = 0.1;    
    float zFar  = 400.0;
    float depth = texture2D(uTexture,  texCoords).x;
    return (2.0 * zNear) / (zFar + zNear - depth * (zFar - zNear));
}

void main()
{
	float r = linearizeDepth();
	vec4 tex = texture2D(uTexture,  texCoords);
	outputColor = isDepth == 1 ? vec4(r, r, r, 1) : tex;
}
