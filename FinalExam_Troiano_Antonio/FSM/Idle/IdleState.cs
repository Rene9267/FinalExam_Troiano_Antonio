using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExam_Troiano_Antonio
{
    class IdleState : State
    {
        protected Actor actor;
        private string texturename;

        public IdleState(Actor actor, string texturename)
        {
            this.actor = actor;
            this.texturename = texturename;
        }
        public override void OnEnter()
        {
            actor.texture = GfxMgr.GetTexture(texturename);
        }
        public override void Update()
        {
            //else
            //{
            //    if (((Enemy)actor).Target == null || !((Enemy)actor).Target.IsActive)
            //    {
            //        if (((Enemy)actor).Rival != null && ((Enemy)actor).Rival.IsActive)
            //        {
            //            stateMachine.GoTo(StateEnum.FOLLOW);
            //        }
            //        else
            //        {
            //            stateMachine.GoTo(StateEnum.WALK);
            //        }
            //    }
            //}
        }
    }
}
