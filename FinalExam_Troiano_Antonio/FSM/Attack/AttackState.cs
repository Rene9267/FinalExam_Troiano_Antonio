using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExam_Troiano_Antonio
{
    class AttackState : State
    {
        protected Actor actor;
        private string texturename;
        public AttackState(Actor actor, string texturename)
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
        }
    }
}

