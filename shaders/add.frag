uniform sampler2D source0;
uniform sampler2D source1;
uniform float ratio;

in vec2 texCoords;
out varying vec4 frgColor;

void main()
{
    vec4 sourceFragment = texture2D(source1, texCoords.xy);
    vec4 bloomFragment = texture2D(source0, texCoords.xy);
    
    bloomFragment.rgba *= ratio;
    frgColor = sourceFragment + bloomFragment;   
}