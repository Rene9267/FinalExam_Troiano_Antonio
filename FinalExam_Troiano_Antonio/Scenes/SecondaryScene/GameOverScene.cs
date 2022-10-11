using Aiv.Fast2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExam_Troiano_Antonio
{
    class GameOverScene : TitleScene
    {
        protected GameObject background;
        public GameOverScene(string bgTexturePath) : base(bgTexturePath, KeyCode.Y)
        {
        }
        public override void Start()
        {
            LoadAssets();
            background = new GameObject("backgrown");
            background.Position = Game.ScreenCenter;
            background.IsActive = true;
            IsPlaying = true;
        }
        public override void Update()
        {
        }
        public override void Input()
        {
            base.Input();
            if(IsPlaying && Game.Window.GetKey(KeyCode.N))
            {
                IsPlaying = false;
                NextScene = null;
            }
        }
    }
}
