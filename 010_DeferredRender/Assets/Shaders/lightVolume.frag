#version 330

vec4 color;
vec3 position;

uniform sampler2D gPositionSampler;
uniform sampler2D gNormalSampler;
uniform sampler2D gAlbedoSpecSampler;

out vec4 outputColor;


void main()
{
	vec2 xy = gl_Position;

	vec2 texCoords = vec2(0);

	vec3 posExtracted = texture(gPositionSampler, texCoords).rgb;
	vec3 normalExtracted = texture(gNormalSampler, texCoords).rgb;
	vec3 colorExtracted = texture(gAlbedoSpecSampler, texCoords).rgb;

	vec3 resultingColor = colorExtracted;
	vec3 viewDir = normalize(uCameraPos - posExtracted);

	float dist = length(light.pos - posExtracted);
	//if(dist < light.radius)
	//{
		vec3 lightDirection = normalize(light.pos - posExtracted);
		vec3 diffuse = max(dot(normalExtracted, lightDirection), 0.0) * colorExtracted * light.color;

		diffuse = 1 * diffuse * (1.0f / (1 +  0.25 * dist * dist));
		resultingColor += diffuse;
	//}

	outputColor = vec4(resultingColor, 1.0);
}