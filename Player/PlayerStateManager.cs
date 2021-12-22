using Aspera.Player.States;

namespace Aspera.Player
{
    public class PlayerStateManager
    {
        private PlayerObject player;
        public PlayerStateAction CurrentStateAction { get => StateActions[CurrentState]; }
        public PlayerState CurrentState { get; private set; } = PlayerState.Idle;

        public Dictionary<PlayerState, PlayerStateAction> StateActions { get; private set; } = new Dictionary<PlayerState, PlayerStateAction>();

        public PlayerStateManager(PlayerObject playerObject)
        {
            this.player = playerObject;

            StateActions.Add(PlayerState.Idle, new IdlePlayerState(this));
            StateActions.Add(PlayerState.Moving, new PlayerMovementState(this));
        }

        public void TransitionState(PlayerState state)
        {
            if (CurrentStateAction.ToStates.Contains(state) && StateActions[state].FromStates.Contains(CurrentState))
                CurrentState = state;
        }

        public void Update(float delta)
        {
            CurrentStateAction.Update(delta, player);
        }
    }
}