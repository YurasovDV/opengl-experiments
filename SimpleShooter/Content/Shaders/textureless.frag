#version 330

in vec4 color;
in vec3 normal;
in vec3 position;
in vec3 lightPosition;

out vec4 outputColor;

void main()
{
    //vec3 toLight = position - lightPosition;
    vec3 toLight = lightPosition - position;
	
    vec3 toLightNormalized = normalize(toLight);
	float level = dot(toLightNormalized, normal);
	
	float distanceToLight = length(toLight); 
	
	float diffuse = 2 * level / (1 +  0.25 * distanceToLight * distanceToLight) + 0.2;

	outputColor = diffuse * color;
}