using SFML.Graphics;
using SFML.System;

namespace Aspera
{
    public class ParallaxLayer : Drawable
    {
        public float Offset = 0.0f;
        public Texture Texture;
        private float speed;
        internal Sprite sprite;

        public ParallaxLayer(string file, float speed)
        {
            this.speed = speed;
            this.Texture = new Texture(file);
            this.Texture.Repeated = true;

            sprite = new Sprite(Texture);
            sprite.Origin = new Vector2f(0, 0);
        }

        public void Update(float delta, float timeScale)
        {
            Offset += (delta / speed) * timeScale;
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            states.Shader.SetUniform("offset", Offset);
            target.Draw(sprite, states);
        }
    }
}