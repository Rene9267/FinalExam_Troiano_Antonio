using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace FinalExam_Troiano_Antonio
{
    class WalkState : State
    {
        protected Actor actor;
        private string texturename;
        //private RandomTimer checkForVisiblePowerUp;

        public WalkState(Actor actor, string texturename)
        {
            this.actor = actor;
            this.texturename = texturename;
            //checkForVisiblePowerUp = new RandomTimer(0.5f, 1.4f);
        }
        public override void OnEnter()
        {
            //Actor.Rival = null;
            actor.texture = GfxMgr.GetTexture(texturename);
            //Actor.ComputeRandomPoint();
            //checkForVisiblePowerUp.Cancel();
        }

        public override void Update()
        {

        }
    }
}
