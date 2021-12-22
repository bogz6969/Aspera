using Aspera.Engine;
using Aspera.Graphics.UI;
using SFML.Graphics;

namespace Aspera.Screens
{
    public class MainMenuScreen : IGameScreen
    {
        public GameScreenManager Manager { get; set; }
        public List<UIComponent> UIs { get; set; }

        private float offset = 0;

        public Label AddButton(string text)
        {
            var l = new Label(text, 50);
            l.Position = (SFML.System.Vector2f)(Game.Instance.RenderWindow.Size / 2) - l.Size / 2 + new SFML.System.Vector2f(0, offset);

            offset += 60;
            return l;
        }

        public MainMenuScreen()
        {
            UIs = new List<UIComponent>();
            UIs.Add(AddButton("Play"));
            UIs.Add(AddButton("Options"));
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            foreach (var item in UIs)
            {
                target.Draw(item);
            }
        }

        public void Update(float dt)
        {
            foreach (var item in UIs)
            {
                item.Update(dt);
            }
        }
    }
}