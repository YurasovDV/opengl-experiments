#version 400

in vec3 pos_eye;
in vec3 n_eye;

uniform vec3 cameraPos;
uniform samplerCube cube_texture;
uniform mat4 uMV;

out vec4 outputColor;
in vec4 color;

void main()
{
    vec3 I = normalize(pos_eye - cameraPos);
    vec3 reflected = reflect(I, normalize(n_eye));
    outputColor = vec4(texture(cube_texture, reflected).rgb, 1.0);
}