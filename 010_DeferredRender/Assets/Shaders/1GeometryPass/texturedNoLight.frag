#version 330

varying vec4 color;
varying vec3 normal;
varying vec3 position;

varying vec2 v_TexCoordinate;

uniform sampler2D u_Texture;

layout (location = 0) out vec3 gPosition;
layout (location = 1) out vec3 gNormal;
layout (location = 2) out vec4 gAlbedoSpec;

void main()
{
	gPosition = position;
	gNormal = normal;
	gAlbedoSpec.rgb = vec3(texture(u_Texture, v_TexCoordinate)) + vec3(0.3f);
	gAlbedoSpec.a = 1;   
}

