uniform sampler2D texture;

in vec2 texCoords;
out varying vec4 frgColor;

void main()
{
	vec2 offsetFactor = vec2(0.001f);
	vec2 textureCoordinates = texCoords;
	vec4 color = vec4(0.0);
	color += texture2D(texture, textureCoordinates - 4.0 * offsetFactor) * 0.0162162162;
	color += texture2D(texture, textureCoordinates - 3.0 * offsetFactor) * 0.0540540541;
	color += texture2D(texture, textureCoordinates - 2.0 * offsetFactor) * 0.1216216216;
	color += texture2D(texture, textureCoordinates - offsetFactor) * 0.1945945946;
	color += texture2D(texture, textureCoordinates) * 0.2270270270;
	color += texture2D(texture, textureCoordinates + offsetFactor) * 0.1945945946;
	color += texture2D(texture, textureCoordinates + 2.0 * offsetFactor) * 0.1216216216;
	color += texture2D(texture, textureCoordinates + 3.0 * offsetFactor) * 0.0540540541;
	color += texture2D(texture, textureCoordinates + 4.0 * offsetFactor) * 0.0162162162;
	frgColor = color;
}