using SFML.Graphics;
using System.Text;

namespace Aspera.Graphics
{
    public class DebugInformationDrawer : Drawable
    {
        private Text _text;
        private StringBuilder stringBuilder;

        public DebugInformationDrawer()
        {
            _text = new Text("", new Font("C:/Windows/Fonts/arial.ttf"), 13);
            stringBuilder = new StringBuilder();
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            _text.DisplayedString = stringBuilder.ToString();
            target.Draw(_text);
            stringBuilder.Clear();
        }

        internal void AppendText(string text)
        {
            stringBuilder.AppendLine(text);
        }
    }
}