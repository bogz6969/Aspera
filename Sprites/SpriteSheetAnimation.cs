using SFML.Graphics;

namespace Aspera.Sprites
{
    public class SpriteSheetAnimation
    {
        public float FrameTime { get; set; } = 0.1f;
        public int Frame { get; private set; } = 0;
        public List<IntRect> Frames { get; private set; }

        private float _time = 0;

        public SpriteSheetAnimation(List<IntRect> inFrames)
        {
            Frames = inFrames;
        }

        public void Update(float delta)
        {
            _time += delta;
            if (_time >= FrameTime)
            {
                if (Frame + 1 < Frames.Count)
                    Frame++;
                else Frame = 0;
                _time = 0;
            }
        }

        public IntRect GetSprite()
        {
            return Frames[Frame];
        }
    }
}