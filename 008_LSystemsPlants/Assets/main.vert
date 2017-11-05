#version 330

attribute vec3 vPos;
attribute vec3 vNormal;
attribute vec3 vColor;

uniform mat4 uMV;
uniform mat4 uMVP;
uniform mat4 uP;


out vec4 color;
out vec3 pos;
out vec3 normal;


void main()
{
	gl_Position = uMVP * vec4(vPos, 1.0);

	color = vec4(vColor, 1.0);
	pos = vec3(uMVP * vec4(vPos, 1.0f));
	normal =  vec3(uMVP * vec4(vNormal, 1.0f));
}