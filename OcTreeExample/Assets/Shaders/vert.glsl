#version 330
 
in vec3 vPosition;
in vec3 vColor;
out vec4 color;
uniform mat4 u_modelViewprojection;
 
// textures
attribute vec2 a_TexCoordinate; // Per-vertex texture coordinate information we will pass in.
varying vec2 v_TexCoordinate;   // This will be passed into the fragment shader.

void
main()
{
    gl_Position = u_modelViewprojection * vec4(vPosition, 1.0);
    // Pass through the texture coordinate.
    v_TexCoordinate = a_TexCoordinate;
    color = vec4(vColor, 0.0f);
}