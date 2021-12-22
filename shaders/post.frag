
#version 330
uniform sampler2D texture;

void main()
{
    // lookup the pixel in the texture
    
    
    vec4 pixel = texture2D(texture, gl_TexCoord[0].xy);
    
    gl_FragColor = gl_Color * pixel;
}