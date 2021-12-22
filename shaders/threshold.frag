uniform sampler2D texture;
uniform float threshold;
uniform bool negate;

in vec2 texCoords;
out varying vec4 frgColor;

void main()
{
    vec4 pixel = texture2D(texture, texCoords.xy);
    
    if (negate)
    {    
        if ((pixel.r + pixel.g + pixel.b) / 3.0f > threshold) {
        pixel.a = 0.0f;
        discard;
        }
    }
    else
    {
        if ((pixel.r + pixel.g + pixel.b) / 3.0f < threshold) {
        pixel.a = 0.0f;
        discard;
        } 
    }
 
    frgColor = gl_Color * pixel ;
}