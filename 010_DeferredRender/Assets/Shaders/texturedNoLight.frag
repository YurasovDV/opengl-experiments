#version 330

varying vec4 color;
varying vec3 normal;
varying vec3 position;
varying vec2 v_TexCoordinate;

uniform sampler2D u_Texture;

out vec4 outputColor;

void main()
{
    vec4 tex = texture2D(u_Texture, v_TexCoordinate);
    outputColor = tex;
}

