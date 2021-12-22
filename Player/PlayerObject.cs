using Aspera.Sprites;
using Genbox.VelcroPhysics.Dynamics;
using Genbox.VelcroPhysics.Factories;
using SFML.Graphics;
using SFML.System;

namespace Aspera.Player
{
    public enum Direction
    {
        None,
        Left,
        Right,
        Up,
        Down
    }

    public enum PlayerState
    {
        Idle,
        Moving,
        Jumping
    }

    public class PlayerObject : Drawable
    {
        public PlayerStateManager StateManager { get; private set; }

        private static readonly float moveSpeed = 250;
        private Shader shader;
        private SpriteSheet playerSpriteSheet;
        private RenderTexture renderTexture;

        internal void Move(Direction dir, float delta)
        {
            switch (dir)
            {
                case Player.Direction.None:
                    MovementScale = MathHelper.Lerp(MovementScale, 0, delta * 10);
                    playerSpriteSheet.SetAnimation("idle");
                    break;

                case Player.Direction.Left:
                    MovementScale = MathHelper.Lerp(MovementScale, -1, delta * 10);
                    Direction = new Vector2f(-1, 0);
                    Body.Position += (new Microsoft.Xna.Framework.Vector2(-moveSpeed * delta, 0));

                    playerSpriteSheet.SetAnimation("run");
                    break;

                case Player.Direction.Right:
                    MovementScale = MathHelper.Lerp(MovementScale, 1, delta * 10);
                    Direction = new Vector2f(1, 0);
                    Body.Position += (new Microsoft.Xna.Framework.Vector2(moveSpeed * delta, 0));

                    playerSpriteSheet.SetAnimation("run");
                    break;
                case Player.Direction.Up:
                    Direction = new Vector2f(0, -1);
                    Body.Position += (new Microsoft.Xna.Framework.Vector2(0, -moveSpeed * delta));

                    playerSpriteSheet.SetAnimation("run");
                    break;
                case Player.Direction.Down:
                    Direction = new Vector2f(0, 1);
                    Body.Position += (new Microsoft.Xna.Framework.Vector2(0, moveSpeed * delta));

                    playerSpriteSheet.SetAnimation("run");
                    break;
            }
        }

        private Sprite spr;
        public Vector2f Direction { get; private set; } = new Vector2f(0, 0);
        public static Vector2f Size { get; private set; }
        public Body Body { get; private set; }
        public float MovementScale { get; internal set; }

        public bool HasMoved
        {
            get; private set;
        }


        public PlayerObject()
        {
            StateManager = new PlayerStateManager(this);

            playerSpriteSheet = new SpriteSheet(new Texture("assets/player/spritesheet.png"), 50, 37);
            playerSpriteSheet.AddAnimation("idle", 0, 0, 3, 0.2f);
            playerSpriteSheet.AddAnimation("run", 1, 1, 6, 0.1f);
            playerSpriteSheet.AddAnimation("fall", 1, 3, 2, 0.1f);

            Size = new Vector2f(8 * 5, 32 * 3f);

            Body = BodyFactory.CreateRectangle(Game.Instance.World, Size.X, Size.Y, 1.0f);
            Body.BodyType = BodyType.Dynamic;
            Body.Friction = 0.8f;

            Body.FixedRotation = true;
            Body.SleepingAllowed = false;
            Body.Restitution = 0.1f;

            shader = new Shader(null, null, "shaders/player.frag");
            renderTexture = new RenderTexture(50, 37);
            spr = new Sprite();
            spr.Origin = new Vector2f(50 / 2.0f, 37 / 2.0f);
            spr.Texture = renderTexture.Texture;
            spr.Scale = new Vector2f(4, 4);

            Body.Position = new Microsoft.Xna.Framework.Vector2(1000);
            spr.Position = Body.Position.Convert();

        }

        public void SetAnimation(string name)
        {
            playerSpriteSheet.SetAnimation(name);
        }

        private Vector2f pPos;
        private float pPosTimer = 0, pFallTimer = 0;

        public void Update(float delta)
        {
            pPosTimer += delta;
            playerSpriteSheet.Update(delta);
            StateManager.Update(delta);

            spr.Position = Body.Position.Convert() + new Vector2f(0, -36);
            spr.Rotation = MathF.Max(-20, MathF.Min(20, Body.Rotation * 180.0f / 3.1415f));


            if (pPosTimer > 0.3f)
            {
                HasMoved = !MathHelper.CloseEnough(spr.Position, pPos, 0.01f);

                pPosTimer = 0;
                pPos = spr.Position;
            }
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            Game.Instance.DebugInformation.AppendText($"PlayerState: {StateManager.CurrentState}");

            renderTexture.Clear(Color.Transparent);
            renderTexture.Draw(playerSpriteSheet);
            renderTexture.Display();
            shader.SetUniform("dir", Direction);
            states.Shader = shader;
            target.Draw(spr, states);
        }

        internal Vector2f GetPosition()
        {
            return Body.Position.Convert();
        }
    }
}