#version 400

in vec3 textureCoordinates;
out vec4 out_color;

uniform samplerCube cubemap;

void main()
{
	out_color = texture(cubemap, textureCoordinates);
}