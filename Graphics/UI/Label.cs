using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Aspera.Graphics.UI
{
    public class Label : UIComponent
    {
        private Text _text;
        public string Text { get; set; }
        public bool Hovered { get; private set; } = false;
        public bool Pressed { get; private set; } = false;

        public delegate void Clicked(Label sender);

        public event Clicked OnClicked;

        public Vector2f Position
        {
            get
            {
                return _text.Position;
            }
            set
            {
                _text.Position = value;
            }
        }

        public Vector2f Size
        {
            get
            {
                return new(_text.GetLocalBounds().Width, _text.GetLocalBounds().Height);
            }
        }

        public Label(string text, uint size = 24)
        {
            _text = new Text(text, new Font("C:/Windows/Fonts/arial.ttf"), size);
        }

        private bool oldMouseDown = false;

        public override void Update(float dt)
        {
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(_text);
            var mPos = Mouse.GetPosition(Game.Instance.RenderWindow);
            var mClicked = Mouse.IsButtonPressed(Mouse.Button.Left);

            Hovered = _text.GetGlobalBounds().Contains(mPos.X, mPos.Y);

            if (Hovered)
            {
                Pressed = !oldMouseDown && mClicked;
                if (Pressed)
                    OnClicked?.Invoke(this);
            }

            oldMouseDown = mClicked;
        }
    }
}