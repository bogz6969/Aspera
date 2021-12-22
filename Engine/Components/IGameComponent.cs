using SFML.Graphics;

namespace Aspera.Engine.Components
{
    public interface IGameComponent : Drawable
    {
        void Update(float dt);
    }
}