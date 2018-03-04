#version 330

out vec4 outputColor;


uniform sampler2D gPositionSampler;
uniform sampler2D gNormalSampler;
uniform sampler2D gAlbedoSpecSampler;

uniform sampler2D gDiffuseSampler;
uniform sampler2D gSpecularSampler;

uniform vec3 uCameraPos;

in vec2 texCoords;

void main()
{	
	vec3 posExtracted = texture(gPositionSampler, texCoords).rgb;
	vec3 normalExtracted = texture(gNormalSampler, texCoords).rgb;
	vec3 colorExtracted = texture(gAlbedoSpecSampler, texCoords).rgb;

	vec3 diffuseExtracted = texture(gDiffuseSampler, texCoords).rgb;

	vec3 colorExtractedWeakened = colorExtracted * 0.2f;
	vec3 mixed = colorExtracted * diffuseExtracted;
	outputColor = vec4(colorExtractedWeakened + mixed, 1.0);
}