using Aiv.Fast2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExam_Troiano_Antonio
{
    class GoodEndScene : TitleScene
    {
        public GoodEndScene() : base("Assets/aivBG.png", KeyCode.Y)
        {
        }
        public override void Update()
        {
        }
        public override void Start()
        {
        }
        public override void Input()
        {
            base.Input();
            if (IsPlaying && Game.Window.GetKey(KeyCode.N))
            {
                IsPlaying = false;
                NextScene = null;
            }
        }
    }
}
