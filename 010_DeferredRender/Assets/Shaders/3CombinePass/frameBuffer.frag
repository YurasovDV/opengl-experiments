#version 330

out vec4 outputColor;

// material color
uniform sampler2D gAlbedoSpecSampler;

// 2nd pass buffer(lighting)
uniform sampler2D gDiffuseSampler;

in vec2 texCoords;

void main()
{	
	vec3 colorExtracted = texture(gAlbedoSpecSampler, texCoords).rgb;

	vec3 diffuseExtracted = texture(gDiffuseSampler, texCoords).rgb;

	vec3 mixed = colorExtracted * diffuseExtracted;

	outputColor = vec4(mixed, 1.0);
}