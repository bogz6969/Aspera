using SFML.Graphics;

namespace Aspera.Engine
{
    public interface IGameScreen : Drawable
    {
        GameScreenManager Manager { get; set; }

        void Update(float dt);
    }
}