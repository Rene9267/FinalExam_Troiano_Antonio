using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using FinalExam_Troiano_Antonio;

namespace FinalExam_Troiano_Antonio
{
    class Enemy : Actor
    {
        public SoundEmitter ScreamSound;
        public RandomizeSoundEmitter AttackFrase;

        private bool Human;
        private float walkSpeed;
        private StateMachine fsm;
        private float followSpeed;
        private GridPathfinder pathFinder;

        public Enemy(GridPathfinder pf, float scale = 0) : base("GhostIdleFront", DrawLayer.Playground)
        {
            RigidBody = new RigidBody(this);
            RigidBody.Type = RigidBodyType.Enemy;
            RigidBody.Collider = ColliderFactory.CreateBoxFor(this, scale);
            RigidBody.AddCollisionType(RigidBodyType.Player);
            sprite.scale *= scale;

            ScreamSound = new SoundEmitter(this, "MamaScream");
            walkSpeed = 1.5f;
            followSpeed = walkSpeed * 2.0f;
            pathFinder = pf;
            AttackFrase = new RandomizeSoundEmitter(this);
            AttackFrase.AddClip("FindYou");
            AttackFrase.AddClip("Can'tSeeMe");//wella
            AttackFrase.AddClip("DeathIs");

            fsm = new StateMachine();
            fsm.AddState(StateEnum.IDLE, new IdleEnemyState(this));
            fsm.AddState(StateEnum.WALK, new WaitingEnemyState(this));
            fsm.AddState(StateEnum.FOLLOW, new FollowMamaState(this));
            fsm.AddState(StateEnum.HUMAN, new HumanState(this));
            fsm.AddState(StateEnum.SPECIAl, new HugState(this));
            if ((PlayScene)Game.CurrentScene is HomeScene || Game.CurrentScene is EndScene)
                fsm.SetFirstState(StateEnum.IDLE);
            else
                fsm.SetFirstState(StateEnum.WALK);
        }
        public void ComputePlayerPoint()
        {
            pathFinder.SelectPathFromTo(Position, ((PlayScene)Game.CurrentScene).player.Position);
        }
        public void ComputeEndPoint()
        {
            pathFinder.SelectPathFromTo(Position, new Vector2(9, 8));
        }
        public void ComputeSprite()
        {
            if (Game.CurrentScene is EndScene)
            {
                if (RigidBody.Velocity != Vector2.Zero)
                {
                    if (RigidBody.Velocity.X > 1)
                    {
                        sprite.FlipX = false;
                        texture = GfxMgr.GetTexture("HumanMamaSide");
                    }
                    else if (RigidBody.Velocity.X < -1)
                    {
                        sprite.FlipX = true;
                        texture = GfxMgr.GetTexture("HumanMamaSide");
                    }
                    else if (RigidBody.Velocity.Y > -1)
                        texture = GfxMgr.GetTexture("HumanMamaDown");
                    else if (RigidBody.Velocity.Y < 1)
                        texture = GfxMgr.GetTexture("HumanMamaUp");
                }
            }
            else
            if (RigidBody.Velocity != Vector2.Zero)
            {
                if (RigidBody.Velocity.X > 1)
                {
                    sprite.FlipX = false;
                    texture = GfxMgr.GetTexture("MamaRunSide");
                }
                else if (RigidBody.Velocity.X < -1)
                {
                    sprite.FlipX = true;
                    texture = GfxMgr.GetTexture("MamaRunSide");
                }
                else if (RigidBody.Velocity.Y > -1)
                    texture = GfxMgr.GetTexture("MamaRunFront"); 
                else if (RigidBody.Velocity.Y < 1)
                    texture = GfxMgr.GetTexture("MamaRunBack"); 
            }
        }
        public override void OnCollide(Collision collisionInfo)
        {
            if (collisionInfo.Collider is Player)
            {
                if (((PlayScene)Game.CurrentScene) is HomeScene)
                {
                    ScreamSound.Play();
                    OnDie();
                }
                else if (((PlayScene)Game.CurrentScene) is EndScene && !Human)
                {
                    fsm.GoTo(StateEnum.SPECIAl);
                    Human = true;
                }
            }
        }
        public void HeadToPlayer()
        {
            Vector2 dir = pathFinder.NextPathDirection(Position);
            RigidBody.Velocity = followSpeed * dir;
        }
        public override void Update()
        {
            fsm.Update();
            base.Update();
        }
        public virtual void Spawn()
        {
            IsActive = true;
            Position = ((PlayScene)Game.CurrentScene).player.Position + new Vector2(RandomGenerator.GetRandomFloat(-8, 8), RandomGenerator.GetRandomFloat(-8, 8));
            fsm.GoTo(StateEnum.FOLLOW);
        }
        public override void OnDie()
        {
            IsActive = false;
            ((HomeScene)Game.CurrentScene).KeyOutdoor.IsActive = true;
            ((HomeScene)Game.CurrentScene).Carillon.IsActive = true;
        }
        public override void Draw()
        {
            if (!IsActive) return;
            if (animation.IsEnabled)
                sprite.DrawTexture(texture, (int)animation.Offset.X, (int)animation.Offset.Y, animation.FrameWidth, animation.FrameHeight);
            else base.Draw();
        }
    }
}
