#version 330

uniform mat4 uMVP;

attribute vec3 vColor;
attribute vec3 vPosition;

out vec4 color;

void main()
{
	gl_Position = uMVP * vec4(vPosition, 1.0);
	color = vec4(vColor, 1.0);
}