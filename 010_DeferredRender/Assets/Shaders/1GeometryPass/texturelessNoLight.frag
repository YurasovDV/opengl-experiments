#version 330

varying vec4 color;
varying vec3 normal;
varying vec3 position;

layout (location = 0) out vec3 gPosition;
layout (location = 1) out vec3 gNormal;
layout (location = 2) out vec4 gAlbedoSpec;

void main()
{
    gPosition = position;
	gNormal = normal;
	gAlbedoSpec.rgb = color.rgb;
	gAlbedoSpec.a = 1;   
}