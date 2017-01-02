#version 330

varying vec4 color;
varying vec3 normal;
varying vec3 position;
varying vec3 lightPosition;
varying vec4 fragInLightSpace;

out vec4 outputColor;

uniform sampler2D uTexture;

uniform sampler2D uShadowMap;

uniform int noTextureFlag;

varying vec2 texCoordinate;


float not_in_shadow(vec4 point)
{
	float inShadow = 0;
	float notInShadow = 1;

	vec3 projected = point.xyz / point.w; 
	projected = projected * 0.5 + 0.5;

	float closestDepthToLight = texture2D(uShadowMap, projected.xy).r;
	float currentDepth = projected.z;

	float shadow = currentDepth > closestDepthToLight ? inShadow : notInShadow;	
	shadow = currentDepth >= 1.0f || currentDepth <= 0f ? notInShadow : shadow;
	return shadow;
}

void main()
{
    //vec3 toLight = lightPosition - position;
    //float distance = length(toLight);
    //vec3 lightVector = normalize(toLight);
    float diffuse =1;// dot(lightVector, normalize(normal));
    diffuse = 1;// 400 * diffuse * (1.0f / (1 +  0.25 * distance * distance));

	float notInShadow = not_in_shadow(fragInLightSpace);
	diffuse = diffuse *  notInShadow + 0.15f;

	outputColor = diffuse * (noTextureFlag != 1 ? texture2D(uTexture,  texCoordinate) : color);
}
