#version 330

out vec4 outputColor;

// material color
uniform sampler2D gAlbedoSpecSampler;

// 2nd pass buffer(lighting)
uniform sampler2D gDiffuseSampler;

uniform vec3 uCameraPos;

in vec2 texCoords;

void main()
{	
	vec3 colorExtracted = texture(gAlbedoSpecSampler, texCoords).rgb;

	vec3 diffuseExtracted = texture(gDiffuseSampler, texCoords).rgb;

	vec3 colorExtractedWeakened = colorExtracted * 0.2f;
	vec3 mixed = colorExtracted * diffuseExtracted;
	outputColor = vec4(colorExtractedWeakened + mixed, 1.0);
}