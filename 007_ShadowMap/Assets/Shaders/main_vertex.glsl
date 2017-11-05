#version 330

uniform mat4 uMV;
uniform mat4 uMVP;
uniform mat4 uProjection;

uniform mat4 uLightSpaceMVP;

uniform vec3 uLightPos;
uniform vec3 uLightDir;

attribute vec3 vPosition;
attribute vec3 vNormal;
attribute vec3 vColor;

attribute vec2 vTexCoordinate; 
varying vec2 texCoordinate;  

out vec4 color;
out vec3 normal;
out vec3 position;
out vec3 lightPosition;
out vec3 lightDirection;

out vec4 fragInLightSpace;

void main()
{
    gl_Position = uMVP * vec4(vPosition, 1.0);

    color = vec4(vColor, 1.0);
    position = vec3(uMV * vec4(vPosition, 1.0)); 
    lightPosition = vec3(uMV * vec4(uLightPos, 1.0)); 
	lightDirection = vec3(uMV * vec4(uLightDir, 1.0));

    normal = vec3(uMV * vec4(vNormal, 0.0));
	texCoordinate = vTexCoordinate;

	fragInLightSpace = uLightSpaceMVP * vec4(vPosition, 1.0);
}