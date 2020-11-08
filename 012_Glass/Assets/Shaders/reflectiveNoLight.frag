#version 400

in vec3 pos_eye;
in vec3 n_eye;

uniform samplerCube cube_texture;
uniform mat4 uMV;

out vec4 outputColor;
varying vec4 color;

void main()
{
  vec3 reflected = reflect(normalize(pos_eye), normalize(n_eye));
  reflected = vec3(inverse(uMV) * vec4(reflected, 0.0));
  outputColor = texture(cube_texture, reflected); // color;
}