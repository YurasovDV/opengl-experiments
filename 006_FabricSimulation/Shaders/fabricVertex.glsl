#version 330

uniform mat4 uMVP;
uniform mat4 uMV;
uniform vec3 uLight;

attribute vec3 vNormal;

attribute vec3 vPos;
attribute vec3 vColor;


out vec4 color;
out vec3 normal;
out vec3 position;
out vec3 lightPosition;

void main()
{
	gl_Position = uMVP * vec4(vPos, 1.0);
	color = vec4(vColor, 1.0);
	position = vec3(uMV * vec4(vPos, 0.0));
	normal = vec3(uMV * vec4(vNormal, 0.0));
	lightPosition = vec3(uMV * vec4(uLight, 0.0));
}