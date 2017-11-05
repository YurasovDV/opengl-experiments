#version 330

uniform mat4 uMVP;
uniform mat4 uMV;
uniform mat4 uP;

uniform vec3 uLightPos;

attribute vec3 vPosition;
attribute vec3 vColor;
attribute vec3 vNormal;

out vec4 color;
out vec3 normal;
out vec3 position;
out vec3 lightPosition;

void main()
{
   gl_Position = uMVP * vec4(vPosition, 1.0);
   color = vec4(vColor, 1.0);

   position = vec3(uMV * vec4(vPosition, 0.0));
   normal = vec3(uMV * vec4(vNormal, 0.0));
   lightPosition = vec3(uMV *  vec4(uLightPos, 0.0));
}