using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExam_Troiano_Antonio
{
    class WaitingEnemyState : WalkState
    {
        private RandomTimer checkForVisiblePlayer;
        public WaitingEnemyState(Enemy enemy) : base(enemy, "GhostIdleFront")
        {
            checkForVisiblePlayer = new RandomTimer(10, 20);
        }
        public override void OnEnter()
        {
            checkForVisiblePlayer.Reset();
        }
        public override void Update()
        {
            if (((PlayScene)Game.CurrentScene).player == null) return;
            checkForVisiblePlayer.Tick();
            if (checkForVisiblePlayer.IsOver())
            {
                ((Enemy)actor).Spawn();
            }
        }
    }
}
