
vec3 color;
vec3 pos;
vec3 normal;

out vec4 outputColor;

void main()
{
	outputColor = vec4(color, 1.0f);
}