#version 330

uniform mat4 uMV;
uniform mat4 uMVP;
uniform mat4 uTransform;

attribute vec3 vPosition;

void main()
{
	vec4 posInWorld = uTransform * vec4(vPosition, 1.0);

	gl_Position = uMVP * vec4(posInWorld.xyz, 1.0);
}