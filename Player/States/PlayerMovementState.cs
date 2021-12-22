using SFML.Window;

namespace Aspera.Player.States
{
    public class PlayerMovementState : PlayerStateAction
    {
        public PlayerMovementState(PlayerStateManager manager) : base(manager)
        {
            FromStates = new List<PlayerState>
            {
                PlayerState.Idle
            };
            ToStates = new List<PlayerState>
            {
                PlayerState.Idle
            };
        }

        public override void Update(float delta, PlayerObject player)
        {
            bool key = false;
            if (Keyboard.IsKeyPressed(Keyboard.Key.A))
            {
                key = true;
                player.Body.ApplyLinearImpulse(new Microsoft.Xna.Framework.Vector2(-0.01f, 0));
                player.Move(Direction.Left, delta);
            }
            else if (Keyboard.IsKeyPressed(Keyboard.Key.D))
            {
                key = true;
                player.Body.ApplyLinearImpulse(new Microsoft.Xna.Framework.Vector2(0.01f, 0));
                player.Move(Direction.Right, delta);
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.W))
            {
                key = true;
                player.Body.ApplyLinearImpulse(new Microsoft.Xna.Framework.Vector2(0, -0.01f));
                player.Move(Direction.Up, delta);
            }
            else if (Keyboard.IsKeyPressed(Keyboard.Key.S))
            {
                key = true;
                player.Body.ApplyLinearImpulse(new Microsoft.Xna.Framework.Vector2(0, 0.01f));
                player.Move(Direction.Down, delta);
            }


            if (!key)
            {
                player.Move(Direction.None, delta);
                if (!player.HasMoved)
                {
                    Manager.TransitionState(PlayerState.Idle);
                }
            }
        }
    }
}