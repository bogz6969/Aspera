using SFML.Graphics;
using SFML.System;

namespace Aspera
{
    public class ParallaxManager : Drawable
    {
        public Sprite Sprite { get; set; }
        private RenderTexture renderTexture;

        public static Shader? parallaxShader;

        public float TimeScale { get; set; } = 1.0f;
        public List<ParallaxLayer> Layers { get; set; } = new List<ParallaxLayer>();

        internal void AddLayer(string file, float speed)
        {
            var layer = new ParallaxLayer(file, speed);
            //layer.sprite.Scale = Sprite.Scale;
            layer.sprite.Position = new Vector2f(0, 0);
            layer.sprite.Origin = new Vector2f(0, 0);
            Layers.Add(layer);
        }

        public ParallaxManager()
        {
            renderTexture = new RenderTexture(Game.Instance.RenderWindow.Size.X, Game.Instance.RenderWindow.Size.Y);
            Sprite = new Sprite();
            Sprite.Texture = renderTexture.Texture;
            Sprite.Origin = new(0, 0);
            if (parallaxShader == null)
            {
                parallaxShader = Shader.FromString(File.ReadAllText("shaders/parallax.vert"), null, null);
            }
        }

        public void Update(float delta)
        {
            foreach (var layer in Layers)
                layer.Update(delta, TimeScale);
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            renderTexture.Clear();
            states.Shader = parallaxShader;
            foreach (var layer in Layers)
                renderTexture.Draw(layer, states);
            renderTexture.Display();

            target.Draw(Sprite, RenderStates.Default);
        }
    }
}