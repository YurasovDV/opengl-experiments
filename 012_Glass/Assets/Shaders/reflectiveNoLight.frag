#version 400

uniform vec3 cameraPos;
uniform samplerCube cube_texture;
uniform mat4 uMV;

in vec3 pos_eye;
in vec3 n_eye;
in vec4 color;

out vec4 outputColor;

void main()
{
    vec3 I = normalize(pos_eye - cameraPos);
    vec3 reflected = reflect(I, normalize(n_eye));
    outputColor = mix(vec4(0, 0, 0.5, 1), vec4(texture(cube_texture, reflected).rgb, 1), 0.8); 
    //outputColor = texture(cube_texture, pos_eye);
}