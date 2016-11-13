#version 330

in vec3 textureCoordinates;
out vec4 out_color;

uniform samplerCube cubemap;

void main()
{
	float light = 1f;
	out_color = texture(cubemap, textureCoordinates)*vec4(light, light, light, 1);
}