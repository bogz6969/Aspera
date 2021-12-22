using Aspera.Engine.Components;
using SFML.Graphics;

namespace Aspera.Graphics.UI
{
    public class UIComponent : IGameComponent
    {
        public virtual void Draw(RenderTarget target, RenderStates states)
        {
        }

        public virtual void Update(float dt)
        {
        }
    }
}