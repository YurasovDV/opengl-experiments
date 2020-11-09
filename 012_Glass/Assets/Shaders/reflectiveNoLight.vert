#version 400

uniform mat4 uMVP;
uniform mat4 uMV;
uniform mat4 uP;
uniform vec3 cameraPos;

in vec3 vPosition;
in vec3 vColor;
in vec3 vNormal;

out vec4 color;
out vec3 pos_eye;
out vec3 n_eye;

void main()
{
   gl_Position = uMVP * vec4(vPosition, 1.0);
   color = vec4(vColor, 1.0);
   pos_eye = vPosition;
   n_eye = vNormal;
}