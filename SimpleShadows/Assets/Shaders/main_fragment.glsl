#version 330

varying vec4 color;
varying vec3 normal;
varying vec3 position;
varying vec3 lightPosition;

out vec4 outputColor;

uniform sampler2D uTexture;

varying vec2 texCoordinate;


void 
main()
{
    vec3 toLight = lightPosition - position;
    float distance = length(toLight);
    vec3 lightVector = normalize(toLight);
    float diffuse = dot(lightVector, normalize(normal));
    diffuse = max(diffuse, 0.1f);
    diffuse = 1300 * diffuse * (1.0f / (1 +  0.25 * distance * distance));
    diffuse = min(diffuse, 0.8);

	vec4 tex = texture2D(uTexture,  texCoordinate);

	//outputColor = color * diffuse;
	outputColor = tex * diffuse;
}
