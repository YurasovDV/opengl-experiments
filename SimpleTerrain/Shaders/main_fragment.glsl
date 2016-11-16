#version 330

varying vec4 color;
varying vec3 normal;
varying vec3 position;
varying vec3 lightPosition;

out vec4 outputColor;


void 
main()
{
    vec3 toLight = lightPosition - position;
    float distance = length(toLight);
    vec3 lightVector = normalize(toLight);
    float diffuse = dot(lightVector, normalize(normal));
    diffuse = max(diffuse, 0.1f);
    diffuse = 1300 * diffuse * (1.0f / (1 +  0.25 * distance * distance));
    diffuse = min(diffuse, 10);

	outputColor = color * diffuse;
}
