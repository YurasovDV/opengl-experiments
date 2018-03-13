#version 330

uniform mat4 uMV;
uniform mat4 uMVP;

attribute vec3 vPosition;

void main()
{
	gl_Position = uMVP * vec4(vPosition, 1.0);
}