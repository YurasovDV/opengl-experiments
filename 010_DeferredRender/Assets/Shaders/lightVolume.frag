#version 330

vec4 color;
vec3 position;

uniform sampler2D gPositionSampler;
uniform sampler2D gNormalSampler;
uniform sampler2D gAlbedoSpecSampler;

uniform vec3 lightPos;
uniform vec3 lightColor;

out vec4 outputColor;

void main()
{
	// vec2 xy = gl_Position;
	float x = gl_FragCoord.x / 1920.0f;
	float y = gl_FragCoord.y / 1080;
	vec2 texCoords = vec2(x, y);

	vec3 posExtracted = texture(gPositionSampler, texCoords).rgb;
	vec3 normalExtracted = texture(gNormalSampler, texCoords).rgb;
	vec3 colorExtracted = texture(gAlbedoSpecSampler, texCoords).rgb;

	vec3 resultingColor = colorExtracted;

	float dist = length(lightPos - posExtracted);

	vec3 lightDirection = normalize(lightPos - posExtracted);
	vec3 diffuse = max(dot(normalExtracted, lightDirection), 0.0) * colorExtracted * lightColor;

	diffuse = 1 * diffuse * (1.0f / (1 +  0.25 * dist * dist));
	resultingColor += diffuse;

	outputColor = vec4(resultingColor, 1.0);
}