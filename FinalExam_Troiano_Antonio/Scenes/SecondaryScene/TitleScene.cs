using Aiv.Fast2D;
using Aiv.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExam_Troiano_Antonio
{
    class TitleScene : Scene
    {
        private GrayScalePFX Gray;
        protected GameObject background;
        protected SoundEmitter MamaSingSound;
        protected SoundEmitter mamaCarillon;
        private float timer = 20;
        private Items items;

        public TitleScene(string bgTexturePath, KeyCode exitKey = KeyCode.Return)
        {
        }
        public override void Start()
        {
            LoadAssets();

            //Gray = new GrayScalePFX();
            //Game.Window.AddPostProcessingEffect(Gray);

            items = new Items("GhostIdleSideWife");
            items.IsActive = true;

            MamaSingSound = new SoundEmitter(items, "GhostSing");
            MamaSingSound.IsEnabled = true;

            mamaCarillon = new SoundEmitter(background, "Carillon");
            mamaCarillon.IsEnabled = true;
            mamaCarillon.Play();

            background = new GameObject("backgrown");
            background.Position = Game.ScreenCenter;
            background.IsActive = true;

            base.Start();
        }
        public void LoadAssets()
        {
            GfxMgr.AddTexture("GhostIdleSideWife", "Assets/GhostWife/IdleSprite/GhostWife_Idle_Side_HumanForm.gif");
            GfxMgr.AddClip("GhostSing", "Assets/AudioClip/MamaGhost/MamaSong.wav");
            GfxMgr.AddClip("Carillon", "Assets/AudioClip/MamaGhost/MamaCarillon.wav");
            GfxMgr.AddTexture("backgrown", "Assets/Levels/aivBG.png");
        }
        public override void Draw()
        {
            DrawMgr.Draw();
        }
        public override void Update()
        {
            UpdateMgr.Update();
            timer -= Game.DeltaTime;
            if (timer <= 18 && timer >= 17)
            {
                MamaSingSound.Play();
                //Game.Window.AddPostProcessingEffect(Gray);
            }
            if (timer <= 0 || Game.Window.GetKey(KeyCode.Space))
            {
                OnExit();
            }
        }
        public override void Input()
        {

        }

        public override Scene OnExit()
        {
            background = null;
            items = null;
            MamaSingSound = null;
            //MamaSingSound.IsEnabled = false;
            mamaCarillon = null;
            UpdateMgr.ClearAll();
            GfxMgr.ClearAll();
            DrawMgr.ClearAll();
            PhysicsMgr.ClearAll();
            return base.OnExit();
        }
    }
}
