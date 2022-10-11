using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExam_Troiano_Antonio
{
    class IdleEnemyState : IdleState
    {
        public IdleEnemyState(Enemy enemy) : base(enemy, "GhostIdleSideWife")
        {
            actor = enemy;
        }
        public override void OnEnter()
        {
            if (Game.CurrentScene is EndScene) ((Enemy)actor).texture = GfxMgr.GetTexture("GhostIdleFront");
            else base.OnEnter();
        }
        public override void Update()
        {
            base.Update();
        }
    }
}
