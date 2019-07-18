#version 330

uniform mat4 u_modelview;
uniform mat4 u_modelViewprojection;
uniform mat4 u_projection;
uniform vec3 vLightPosition;

attribute vec3 vPosition;
attribute vec3 vNormal;
attribute vec3 vColor;


// textures
attribute vec2 a_TexCoordinate;
varying vec2 v_TexCoordinate;  

out vec4 color;
out vec3 normal;
out vec3 position;
out vec3 lightPosition;

void 
main()
{
    gl_Position = u_modelViewprojection * vec4(vPosition, 1.0);
    color = vec4(vColor, 1.0);
    position = vec3(u_modelview * vec4(vPosition, 0.0)); 
    lightPosition = vec3(u_modelview * vec4(vLightPosition, 0.0)); 
    normal = vec3(u_modelview * vec4(vNormal, 0.0));

    v_TexCoordinate = a_TexCoordinate;
}