using Aspera.Engine;
using Aspera.Environment;
using Aspera.Graphics;
using Aspera.Screens;
using ImGuiNET;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Aspera
{
    public class Game
    {
        public static Game Instance
        {
            get
            {
                return _game;
            }
        }

        private static Game _game;
        public GameScreenManager GameScreenManager { get; private set; }
        public RenderWindow? RenderWindow { get; private set; }
        public GameWorld World { get; set; }
        public DebugInformationDrawer DebugInformation { get; private set; }
        public float GameTime { get; private set; }

        public Game()
        {
            _game = this;

            InitializeWindow();
            DebugInformation = new DebugInformationDrawer();
            GameScreenManager = new GameScreenManager();
            GameScreenManager.ChangeScreen(new GameScreen());
        }

        private void InitializeWindow()
        {
            VideoMode mode = new VideoMode(1920, 1080);
            ContextSettings settings = new ContextSettings()
            {
                MajorVersion = 4,
                MinorVersion = 0,
                AntialiasingLevel = 8
            };

            RenderWindow = new RenderWindow(mode, "Aspera", Styles.Titlebar | Styles.Close | Styles.Fullscreen, settings);
            RenderWindow.SetVerticalSyncEnabled(true);
            RenderWindow.Closed += (o, e) => { RenderWindow.Close(); };
        }

        public void StartGame()
        {
            Task.Run(UpdateLoop);

            while (RenderWindow.IsOpen)
            {
                RenderWindow.DispatchEvents();
                Draw();
            }
        }

        public async Task UpdateLoop()
        {
            Clock clock = new Clock();

            while (true)
            {
                float delta = clock.Restart().AsSeconds();
                GameTime += delta;

                Game.Instance.Update(delta);
            }
        }

        public void Draw()
        {
            RenderWindow.Draw(GameScreenManager);
            RenderWindow.Display();
        }

        public void Update(float delta)
        {
            GameScreenManager.Update(delta);
        }
    }
}