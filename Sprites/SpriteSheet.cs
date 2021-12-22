using SFML.Graphics;

namespace Aspera.Sprites
{
    public class SpriteSheet : Drawable
    {
        public Dictionary<string, SpriteSheetAnimation> Animations { get; private set; } = new();
        public string CurrentAnimation { get; private set; } = "";
        public Texture Texture { get; private set; }

        public int FramesX { get; private set; }
        public int FramesY { get; private set; }

        public Sprite Sprite { get; set; }

        public SpriteSheet(Texture texture, int sizeX, int sizeY)
        {
            this.FramesX = sizeX;
            this.FramesY = sizeY;
            this.Texture = texture;
            Sprite = new Sprite(Texture);
        }

        public void Update(float delta)
        {
            Animations[CurrentAnimation].Update(delta);
        }

        public bool SetAnimation(string name)
        {
            if (!Animations.ContainsKey(name)) return false;

            CurrentAnimation = name;
            return true;
        }

        public void AddAnimation(string name, int posX, int posY, int length, float time = 0.1f)
        {
            if (CurrentAnimation == "") CurrentAnimation = name;

            List<IntRect> frames = new();

            for (int x = 0; x < length; x++)
            {
                frames.Add(new IntRect((posX + x) * FramesX, posY * FramesY, FramesX, FramesY));
            }

            SpriteSheetAnimation anim = new SpriteSheetAnimation(frames)
            {
                FrameTime = time
            };
            Animations.Add(name, anim);
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            Sprite.TextureRect = Animations[CurrentAnimation].GetSprite();
            target.Draw(Sprite, states);
        }
    }
}