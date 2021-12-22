namespace Aspera.Player.States
{
    public class PlayerStateAction
    {
        protected PlayerStateManager Manager { get; private set; }

        public PlayerStateAction(PlayerStateManager manager)
        {
            this.Manager = manager;
        }

        public List<PlayerState> FromStates { get; protected set; }
        public List<PlayerState> ToStates { get; protected set; }

        public virtual void Update(float delta, PlayerObject player)
        {
        }
    }
}