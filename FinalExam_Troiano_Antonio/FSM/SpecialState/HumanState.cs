using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExam_Troiano_Antonio
{
    class HumanState : FollowState
    {
        public HumanState(Enemy enemy) : base(enemy, "GhostIdleFront")
        {
            actor = enemy;
        }
        public override void OnEnter()
        {
            ((Enemy)actor).animation.NumFrames = 4;
            ((Enemy)actor).animation.IsEnabled = true;
            ((PlayScene)Game.CurrentScene).player.Input();
        }
        public override void Update()
        {
            ((Enemy)actor).ComputeEndPoint();
            ((PlayScene)Game.CurrentScene).player.ComputeEndDoorPoint();
            ((PlayScene)Game.CurrentScene).player.HeadToPoint();
            ((Enemy)actor).HeadToPlayer();
            ((Enemy)actor).ComputeSprite();
            ((PlayScene)Game.CurrentScene).player.ComputeSprite();
        }
    }
}
