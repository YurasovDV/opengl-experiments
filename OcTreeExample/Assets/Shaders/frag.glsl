#version 330
 
in vec4 color;
out vec4 outputColor;

//textures
uniform sampler2D u_Texture; // The input texture.
varying vec2 v_TexCoordinate; // Interpolated texture coordinate per fragment.

void
main()
{
   
    vec4 tex = texture2D(u_Texture, v_TexCoordinate);
   // vec4 tempColor = vec4(color.rgb, 1);

	vec2 center = vec2(0.5f, 0.5f);

	float d = distance(v_TexCoordinate, center) * 6;
    
	float intense = 1 /(exp(d * d));

    vec4 tempColor = vec4(color.b * intense, color.b * intense, color.b* intense + color.b, 0.9);

        //discard;
       /* float rg = max(color.r, color.g);
        float rgb = max(rg, color.b);
        tempColor = vec4(rgb, rgb, rgb, 1);*/

    outputColor =  tempColor * tex;
    outputColor.a = tex.a;

}