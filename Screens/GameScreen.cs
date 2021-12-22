using Aspera.Engine;
using Aspera.Environment;
using Aspera.Graphics;
using Aspera.Player;
using SFML.Graphics;
using SFML.System;

namespace Aspera.Screens
{
    public class GameScreen : IGameScreen
    {
        private Sprite spr;
        private Texture vignette;
        public GameScreen()
        {
            InitializeWorld();
            InitializeBuffers();
            InitializeShaders();
            InitializeCamera();
            InitializePlayer();

            vignette = new Texture("assets/overlays/vig.png");
            spr = new Sprite
            {
                Texture = vignette,
                Color = new Color(255, 255, 255, 200),
                Scale = new Vector2f(1.5f, 1.5f)
            };
        }

        public View Camera { get; private set; }
        public GameScreenManager Manager { get; set; }
        public PlayerObject Player { get; private set; }
        public FrameBufferObject PostEffectsFBO { get; private set; }
        public FrameBufferObject AlbedoFBO { get; private set; }
        public EffectStack EffectsStack { get; private set; }
        public GameWorld World { get; private set; }
        public WorldRenderer WorldRenderer { get; private set; }
        public void Draw(RenderTarget target, RenderStates states)
        {
            AlbedoFBO.RenderTexture.Clear();
            //PostEffectsFBO.RenderTexture.Clear();

            AlbedoFBO.RenderTexture.SetView(Game.Instance.RenderWindow.DefaultView);
            AlbedoFBO.RenderTexture.SetView(Camera);
            WorldRenderer.Draw(AlbedoFBO.RenderTexture, Camera, Player);

            AlbedoFBO.RenderTexture.Display();

            //PostEffectsFBO.RenderTexture.Draw(new Sprite(AlbedoFBO.RenderTexture.Texture), RenderStates.Default);
            //PostEffectsFBO.RenderTexture.Display();

            //EffectsStack.Draw(PostEffectsFBO);
            //EffectsStack.Add(AlbedoFBO, 0.5f);

            target.Clear();
            target.Draw(new Sprite(AlbedoFBO.RenderTexture.Texture));
            target.Draw(spr);

            target.Draw(Game.Instance.DebugInformation, RenderStates.Default);
        }

        public void Update(float delta)
        {
            World.Update(delta);
            Player.Update(delta);
            Camera.Center = MathHelper.Lerp(Camera.Center, Player.GetPosition(), 3 * delta);
        }


        private void InitializeBuffers()
        {
            AlbedoFBO = new FrameBufferObject();
            PostEffectsFBO = new FrameBufferObject();
        }

        private void InitializeCamera()
        {
            Camera = new View(new Vector2f(), new Vector2f(Game.Instance.RenderWindow.Size.X, Game.Instance.RenderWindow.Size.Y));
        }

        private void InitializePlayer()
        {
            Player = new PlayerObject();
            Camera.Center = Player.GetPosition();
        }
        private void InitializeShaders()
        {
            //EffectsStack = new EffectStack(Game.Instance.RenderWindow.Size.X, Game.Instance.RenderWindow.Size.Y);

            //EffectsStack.Effects.Add(new Shader("shaders/basic.vert", null, "shaders/threshold.frag"));
            //EffectsStack.GetLastestShader().SetUniform("threshold", 0.5f);
            //EffectsStack.Effects.Add(new Shader("shaders/basic.vert", null, "shaders/blur.frag"));
        }
        private void InitializeWorld()
        {
            World = new GameWorld();
            World.Load();
            WorldRenderer = new WorldRenderer(World);
            Game.Instance.World = World;
        }
    }
}