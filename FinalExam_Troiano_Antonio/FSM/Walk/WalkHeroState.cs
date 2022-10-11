using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
namespace FinalExam_Troiano_Antonio
{
    class WalkHeroState : WalkState
    {
        private float time;
        public WalkHeroState(Player player) : base(player, "HeroWalkFront")
        {
            time = 0.3f;
        }
        public override void OnEnter()
        {
            ((Player)actor).animation.IsEnabled = true;
        }
        public override void Update()
        {
            time -= Game.DeltaTime;
            if (time <= 0)
            {
                ((Player)actor).ComputeStepsSound();
                time += 0.3f;
            }
            ((Player)actor).ComputePoint();
            ((Player)actor).HeadToPoint();
            ((Player)actor).ComputeSprite();

            if (((Player)actor).RigidBody.Velocity == Vector2.Zero)
            {
                if (((PlayScene)Game.CurrentScene) is Beatch_Room && PlayScene.MiniMap.OpenedMap.IsActive)
                    stateMachine.GoTo(StateEnum.IDLE);
                else stateMachine.GoTo(StateEnum.IDLE);
            }
        }
        public override void OnExit()
        {
            base.OnExit();
            ((Player)actor).animation.IsEnabled = false;
            ((PlayScene)Game.CurrentScene).MouseAttualPosition = Vector2.Zero;
        }
    }
}
