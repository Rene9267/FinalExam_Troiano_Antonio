using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExam_Troiano_Antonio
{
    class HugState : State
    {
        public float timeToHug;
        protected Actor actor;
        public HugState(Actor actor)
        {
            this.actor = actor;
            timeToHug = 3;
        }
        public override void OnEnter()
        {
            if (actor is Enemy)
            {
                ((Enemy)actor).texture = GfxMgr.GetTexture("HumanMamaHug");
                ((Enemy)actor).Sprite.FlipX = true;
                ((Enemy)actor).animation.IsEnabled = false;
            }
            if (actor is Player)
            {
                ((Player)actor).texture = GfxMgr.GetTexture("KidHug");
                ((Player)actor).animation.IsEnabled = false;
            }
        }
        public override void Update()
        {
            timeToHug -= Game.DeltaTime;
            if (actor is Enemy)
            {
                if (timeToHug <= 0) stateMachine.GoTo(StateEnum.HUMAN);
            }
            else if (actor is Player)
                if (timeToHug <= 0)
                    stateMachine.GoTo(StateEnum.WALK);
        }
    }
}
