using Aiv.Fast2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExam_Troiano_Antonio
{
    class AngelFeather : Items
    {
        private bool heven;
        private bool gotIt;
        private float time;
        private SoundEmitter ActiveFeatherSound;
        public AngelFeather(float scale=0) : base("AngelFeather",scale)
        {
            ActiveFeatherSound = new SoundEmitter(this, "AngelFeatherSound");
        }
        public override void Update()
        {
            if (!gotIt)
                if (Inventory.Contains(PlayScene.AngelFeather))
                    gotIt = true;

            if (gotIt)
            {
                time += Game.DeltaTime;
                if (Game.Window.GetKey(KeyCode.P)&& time>=7)
                {
                    if (!heven)
                    {
                        ((PlayScene)Game.CurrentScene).Enemy.IsActive = false;
                        ActiveFeatherSound.Play();
                        heven = true;
                    }
                    time = 0;
                }
                else heven = false;
            }
        }
    }
}

