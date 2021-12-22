using Aspera.Engine.Components;
using SFML.Graphics;

namespace Aspera.Engine
{
    public class GameScreenManager : IGameComponent
    {
        private Dictionary<Type, IGameScreen> _gameScreens;

        public IGameScreen CurrentGameScreen { get; private set; }

        public GameScreenManager()
        {
            _gameScreens = new Dictionary<Type, IGameScreen>();
        }

        public void ChangeScreen(IGameScreen screen)
        {
            if (!_gameScreens.ContainsKey(screen.GetType()))
            {
                screen.Manager = this;
                _gameScreens.Add(screen.GetType(), screen);
            }

            CurrentGameScreen = _gameScreens[screen.GetType()];
        }

        public void Update(float dt)
        {
            CurrentGameScreen.Update(dt);
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(CurrentGameScreen);
        }
    }
}