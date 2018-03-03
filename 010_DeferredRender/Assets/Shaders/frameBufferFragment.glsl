#version 330

out vec4 outputColor;


uniform sampler2D gPositionSampler;
uniform sampler2D gNormalSampler;
uniform sampler2D gAlbedoSpecSampler;

uniform vec3 uCameraPos;

in vec2 texCoords;

struct LightPoint{
	vec3 pos;
	vec3 color;
	float radius;
};

uniform LightPoint light;

void main()
{	
	vec3 posExtracted = texture(gPositionSampler, texCoords).rgb;
	vec3 normalExtracted = texture(gNormalSampler, texCoords).rgb;
	vec3 colorExtracted = texture(gAlbedoSpecSampler, texCoords).rgb;
	colorExtracted = colorExtracted * 0.35f;
	outputColor = vec4(colorExtracted, 1.0);
}
