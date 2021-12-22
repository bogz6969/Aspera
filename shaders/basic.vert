uniform float iTime;

out varying vec2 texCoords;

void main()
{
    // transform the vertex position
    gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;

    // transform the texture coordinates
    texCoords = gl_TextureMatrix[0] * gl_MultiTexCoord0;
    
    // forward the vertex color
    gl_FrontColor = gl_Color;
}