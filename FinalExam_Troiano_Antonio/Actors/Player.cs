using OpenTK;
using System;
using Aiv.Fast2D;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledPlugin;

namespace FinalExam_Troiano_Antonio
{
    class Player : Actor
    {
        public bool IsAlive;

        private bool up;
        private bool down;
        private bool left;
        private bool right;
        private StateMachine fsm;
        private float speed = 3f;
        private WeaponsGUI weaponsGUI;
        private TileObj Tile_Player_Pos;
        private SoundEmitter GrassSteps;
        private SoundEmitter AsphaltSteps;
        private GridPathfinder pathFinder;
        private SoundEmitter SandStepsSound;
        private SoundEmitter WoodStepsSound;

        private Controller controller;
        public Player(Controller ctrl, GridPathfinder pf, float scale = 0) : base("HeroIdleFront", DrawLayer.Playground, Game.PixelsToUnits(16))
        {
            IsAlive = true;
            RigidBody = new RigidBody(this);
            RigidBody.Friction = 40;
            RigidBody.Type = RigidBodyType.Player;
            RigidBody.Collider = ColliderFactory.CreateBoxFor(this, scale);
            RigidBody.AddCollisionType(RigidBodyType.Tile);
            RigidBody.AddCollisionType(RigidBodyType.Enemy);
            sprite.scale *= scale;
            pathFinder = pf;

            weaponsGUI = new WeaponsGUI(Position);
            weaponsGUI.IsActive = true;

            WoodStepsSound = new SoundEmitter(this, "WoodSteps");
            SandStepsSound = new SoundEmitter(this, "SandSteps");
            GrassSteps = new SoundEmitter(this, "GrassSteps");
            AsphaltSteps = new SoundEmitter(this, "AsphaltSteps");

            fsm = new StateMachine();
            fsm.AddState(StateEnum.IDLE, new IdleHeroState(this));
            fsm.AddState(StateEnum.WALK, new WalkHeroState(this));
            fsm.AddState(StateEnum.SPECIAl, new HugState(this));
            fsm.SetFirstState(StateEnum.IDLE);
        }
        public void ComputeStepsSound()
        {
            Tile_Player_Pos = ((PlayScene)Game.CurrentScene).tiles[(int)(pathFinder.ToCell(Position).Y * ((PlayScene)Game.CurrentScene).grid.GetLength(1) + pathFinder.ToCell(Position).X)];
            switch (Tile_Player_Pos.terrain)
            {
                case Terrain.Sand:
                    SandStepsSound.PlayRandom(0.5f);
                    break;
                case Terrain.Wood:
                    WoodStepsSound.PlayRandom(speed);
                    break;
                case Terrain.Grass:
                    GrassSteps.PlayRandom(speed);
                    break;
                case Terrain.Asphalt:
                    WoodStepsSound.PlayRandom(speed);
                    break;
            }
        }
        public void ComputeSprite()
        {
            if (RigidBody.Velocity != Vector2.Zero)
            {
                if (RigidBody.Velocity.X > 1)
                {
                    left = false;
                    right = true;
                    up = false;
                    down = false;
                    sprite.FlipX = false;
                    texture = GfxMgr.GetTexture("HeroWalkSide");
                }
                else if (RigidBody.Velocity.X < -1)
                {
                    left = true;
                    right = false;
                    up = false;
                    down = false;
                    sprite.FlipX = true;
                    texture = GfxMgr.GetTexture("HeroWalkSide");
                }
                else if (RigidBody.Velocity.Y > -1)
                {
                    left = false;
                    right = false;
                    up = true;
                    down = false;
                    sprite.FlipX = false;
                    texture = GfxMgr.GetTexture("HeroWalkFront");
                }
                else if (RigidBody.Velocity.Y < 1)
                {
                    left = false;
                    right = false;
                    up = false;
                    down = true;
                    sprite.FlipX = false;
                    texture = GfxMgr.GetTexture("HeroWalkBack");
                }
            }
        }
        public void ComputeIdleSprie()
        {
            if (right)
            {
                sprite.FlipX = false;
                texture = GfxMgr.GetTexture("HeroIdleSide");
            }
            else if (left)
            {
                sprite.FlipX = true;
                texture = GfxMgr.GetTexture("HeroIdleSide");
            }
            else if (up) texture = GfxMgr.GetTexture("HeroIdleFront");
            else if (down) texture = GfxMgr.GetTexture("HeroIdleBack");
        }
        public void ComputePoint()
        {
            pathFinder.SelectPathFromTo(Position, ((PlayScene)Game.CurrentScene).MouseAttualPosition);
        }
        public void ComputeEndDoorPoint()
        {
            pathFinder.SelectPathFromTo(Position, new Vector2(10, 9));
        }
        public void HeadToPoint()
        {
            Vector2 dir = pathFinder.NextPathDirection(Position);
            RigidBody.Velocity = speed * dir;
        }
        public override void Update()
        {
            fsm.Update();
            base.Update();
        }
        public virtual void Input()
        {
            if (Game.Window.MouseLeft)
            {
                if (!((PlayScene)Game.CurrentScene).IsMousePressed)
                {
                    ((PlayScene)Game.CurrentScene).IsMousePressed = true;
                    ((PlayScene)Game.CurrentScene).MouseAttualPosition =
                        Game.Window.MousePosition + CameraMgr.MainCamera.position - CameraMgr.MainCamera.pivot;
                }
            }
            else ((PlayScene)Game.CurrentScene).IsMousePressed = false;
        }
        public override void Draw()
        {
            if (!IsActive) return;
            if (animation.IsEnabled)
                sprite.DrawTexture(texture, (int)animation.Offset.X, (int)animation.Offset.Y, animation.FrameWidth, animation.FrameHeight);
            else base.Draw();
        }
        public override void OnDie()
        {
            IsAlive = false;
            ((PlayScene)Game.CurrentScene).OnPlayerDies();
        }
        public override void OnCollide(Collision collisionInfo)
        {
            if (collisionInfo.Collider is Enemy && Game.CurrentScene is EndScene)
            {
                fsm.GoTo(StateEnum.SPECIAl);
                Game.Window.ClearPostProcessingEffects();
                ((EndScene)Game.CurrentScene).HellSound.Volume = 0;
            }
            //else if (collisionInfo.Collider is Enemy && !((PlayScene)Game.CurrentScene is HomeScene)) /*OnDie()*/;
            base.OnCollide(collisionInfo);
        }
    }
}
