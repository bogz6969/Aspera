uniform float iTime;

void main()
{
    // transform the vertex position
    gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;

    // transform the texture coordinates
    gl_TexCoord[0] = gl_TextureMatrix[0] * gl_MultiTexCoord0;
    gl_TexCoord[0].x += sin(iTime + gl_TexCoord[0].x * 30.0f) / 100.0f;
    
    // forward the vertex color
    gl_FrontColor = gl_Color;
}