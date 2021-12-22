uniform sampler2D texture;
uniform vec2 dir;

void main()
{
    vec2 texCoords = gl_TexCoord[0].xy;
    if (dir.x == -1) texCoords.x = 1 - texCoords.x;
    vec4 pixel = texture2D(texture, texCoords);

    if (pixel.a == 0.0f) discard;
    
    gl_FragColor = gl_Color * pixel;
}