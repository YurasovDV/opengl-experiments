#version 330

varying vec4 color;
varying vec3 normal;
varying vec3 position;
varying vec3 lightPosition;
out vec4 outputColor;
//textures
uniform sampler2D u_Texture;
// The input texture.
varying vec2 v_TexCoordinate;
// Interpolated texture coordinate per fragment.

void 
main()
{
    vec3 toLight = lightPosition - position;
    float distance = length(toLight);
    vec3 lightVector = normalize(toLight);
    float diffuse = dot(lightVector, normalize(normal));
    diffuse = max(diffuse, 0.1f);
    diffuse = 15 * diffuse * (1.0f / (1 +  0.25 * distance * distance));
    diffuse = min(diffuse, 10);

	vec4 tex = texture2D(u_Texture, v_TexCoordinate);
	// outputColor = color * tex * diffuse;
	outputColor =  tex * diffuse;
}

