using SFML.Graphics;

namespace Aspera.Graphics
{
    public class FrameBufferObject : Drawable
    {
        public Sprite Sprite { get; set; }
        public RenderTexture RenderTexture { get; set; }

        public FrameBufferObject()
        {
            RenderTexture = new RenderTexture(Game.Instance.RenderWindow.Size.X, Game.Instance.RenderWindow.Size.Y);
            Sprite = new Sprite(RenderTexture.Texture);
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(Sprite, states);
        }
    }
}