using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace FinalExam_Troiano_Antonio
{
    class IdleHeroState : IdleState
    {
        public IdleHeroState(Player player) : base(player, "HeroIdleFront")
        {
            actor = player;
        }
        public override void OnEnter()
        {
        }
        public override void Update()
        {
            ((Player)actor).ComputeIdleSprie();
            if (((PlayScene)Game.CurrentScene).MouseAttualPosition != Vector2.Zero)
            {
                if (((PlayScene)Game.CurrentScene) is Beatch_Room && !PlayScene.MiniMap.OpenedMap.IsActive)
                    stateMachine.GoTo(StateEnum.WALK);
                else stateMachine.GoTo(StateEnum.WALK);
            }
        }
    }
}
