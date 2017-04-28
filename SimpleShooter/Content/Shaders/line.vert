#version 330

uniform mat4 uMVP;

varying vec4 vColor;
varying vec3 vPosition;

out vec4 color;

void main()
{
	gl_Position = uMVP * vec4(vPosition, 1.0);
}