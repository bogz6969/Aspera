using Aspera.Engine.Components;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aspera.Graphics
{
    public class EffectStack
    {
        static Shader add;

        static EffectStack()
        {
            add = new Shader("shaders/basic.vert", null, "shaders/add.frag");
        }

        private RenderTexture front, back;
        public List<Shader> Effects { get; private set; } = new List<Shader>();

        public Shader GetLastestShader() => Effects.Last();

        public EffectStack(uint width, uint height)
        {
            front = new RenderTexture(width, height);
            back = new RenderTexture(width, height);

            front.Clear(Color.Transparent);
            back.Clear(Color.Transparent);

            front.Display();
            back.Display();
        }


        public void Draw(Drawable target)
        {
            front.Draw(target);
            foreach (var item in Effects)
            {
                var s = RenderStates.Default;
                s.Shader = item;
                back.Clear(Color.Transparent);
                back.Draw(new Sprite(front.Texture), s);
                back.Display();
                swap();
            }
        }

        public Texture GetTexture() => front.Texture;

        private void swap()
        {
            var old = front;
            front = back;
            back = old;
        }

        internal void Add(FrameBufferObject postEffectsFBO, float ratio = 1.0f)
        {
            var s = RenderStates.Default;
            s.Shader = add;
            back.Clear(Color.Transparent);
            add.SetUniform("ratio", ratio);
            add.SetUniform("source0", front.Texture);
            add.SetUniform("source1", postEffectsFBO.RenderTexture.Texture);
            back.Draw(new Sprite(front.Texture), s);
            back.Display();
            swap();
        }
    }
}
