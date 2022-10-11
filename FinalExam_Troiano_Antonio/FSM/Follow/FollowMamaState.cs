using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExam_Troiano_Antonio
{
    class FollowMamaState : FollowState
    {
        public FollowMamaState(Enemy enemy) : base(enemy, "prova")
        {
        }
        public override void OnEnter()
        {
            ((Enemy)actor).animation.NumFrames = 3;
            ((Enemy)actor).animation.IsEnabled = true;
            ((Enemy)actor).AttackFrase.Play();
        }
        public override void Update()
        {
            if (((PlayScene)Game.CurrentScene).player == null || !((Enemy)actor).IsActive)
                stateMachine.GoTo(StateEnum.WALK);
            else
            {
                ((Enemy)actor).ComputePlayerPoint();
                ((Enemy)actor).HeadToPlayer();
                ((Enemy)actor).ComputeSprite();
            }
        }
    }
}
