#version 330

uniform mat4 uMV;
uniform mat4 uMVP;
uniform vec3 uColor;


attribute vec3 vPosition;

out vec4 color;
out vec3 position;

void main()
{
	gl_Position = uMVP * vec4(vPosition, 1.0);
    color = vec4(vColor, 1.0);
    position = vec3(uMV * vec4(vPosition, 1.0)); 
}