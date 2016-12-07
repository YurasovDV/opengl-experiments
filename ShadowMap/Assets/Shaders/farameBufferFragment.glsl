#version 330

out vec4 outputColor;
uniform sampler2D uTexture;
in vec2 texCoordinate;

void main()
{
	vec4 tex = texture2D(uTexture,  texCoordinate);
	outputColor = tex;
}
