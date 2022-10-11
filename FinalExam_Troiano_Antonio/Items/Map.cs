using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledPlugin;
using Aiv.Fast2D;

namespace FinalExam_Troiano_Antonio
{
    class Map : Items
    {
        public SoundEmitter OpenMapSound;
        public SoundEmitter CloseMapSound;
        private int click = 0;
        public bool MapIsOpen;
        public GameObject OpenedMap;
        private bool MapObtained;
        private bool gotIt;

        public Map(float scale = 1) : base("Map", scale, DrawLayer.Foreground)
        {
            OpenedMap = new GameObject("OpenedMap", DrawLayer.Foreground);
            CloseMapSound = new SoundEmitter(OpenedMap, "CloseMap");
            OpenMapSound = new SoundEmitter(OpenedMap, "OpenMap");
        }
        public void DrawMyMap()
        {
            OpenedMap.IsActive = true;
            OpenMapSound.Play();
        }
        public void DrawNoMoreMyMap()
        {
            CloseMapSound.Play();
            OpenedMap.IsActive = false;
        }
        public override void Update()
        {
            if (!gotIt)
                if (Inventory.Contains(PlayScene.MiniMap))
                    gotIt = true;
            if (gotIt)
            {
                OpenedMap.Position = ((PlayScene)Game.CurrentScene).player.Position;
                if (Game.Window.GetKey(KeyCode.L))
                {
                    if (!MapIsOpen)
                    {
                        MapIsOpen = true;
                        switch (click)
                        {
                            case 1:
                                if (!OpenedMap.IsActive)
                                    DrawMyMap();
                                break;
                            case 2:
                                DrawNoMoreMyMap();
                                click = 0;
                                break;
                        }
                        click += 1;
                    }
                }
                else
                    MapIsOpen = false;
            }
        }
        public override void Draw()
        {
            OpenedMap.Draw();
            base.Draw();
        }
    }
}
