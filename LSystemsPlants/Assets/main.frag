#version 330

varying vec4 color;
varying vec3 pos;
varying vec3 normal;

out vec4 outputColor;

void main()
{
	outputColor = color;
}