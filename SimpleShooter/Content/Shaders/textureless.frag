#version 330

varying vec4 color;
varying vec3 normal;
varying vec3 position;
varying vec3 lightPosition;

out vec4 outputColor;

void main()
{
    //vec3 toLight = position - lightPosition;
    vec3 toLight = lightPosition - position;
	
    vec3 toLightNormalized = normalize(toLight);
	float level = dot(toLightNormalized, normal);
	
	float distanceToLight = length(toLight); 
	
	float diffuse = 100 * level * (1.0f / (1 +  0.25 * distanceToLight * distanceToLight));

    outputColor = diffuse * color;
}