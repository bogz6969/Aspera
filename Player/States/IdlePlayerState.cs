using SFML.Window;

namespace Aspera.Player.States
{
    internal class IdlePlayerState : PlayerStateAction
    {
        public IdlePlayerState(PlayerStateManager manager) : base(manager)
        {
            FromStates = new List<PlayerState>
            {
                PlayerState.Moving
            };
            ToStates = new List<PlayerState>
            {
                PlayerState.Moving
            };
        }

        public override void Update(float delta, PlayerObject player)
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.A) || Keyboard.IsKeyPressed(Keyboard.Key.D) || Keyboard.IsKeyPressed(Keyboard.Key.Space)) Manager.TransitionState(PlayerState.Moving);

            player.Move(Direction.None, delta);
        }
    }
}