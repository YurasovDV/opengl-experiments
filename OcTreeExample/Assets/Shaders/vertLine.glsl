#version 330
 
in vec3 vPosition;
in vec3 vColor;
out vec4 color;
uniform mat4 u_modelViewprojection;

void
main()
{
    gl_Position = u_modelViewprojection * vec4(vPosition, 1.0);
    color = vec4(vColor, 1.0f);
}