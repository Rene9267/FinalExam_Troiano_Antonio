using Aiv.Fast2D;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExam_Troiano_Antonio
{
    class WeaponsGUI : GameObject
    {
        //protected BulletGUIitem[] weapons;
        //protected string[] textureNames = { "bullet_ico", "missile_ico" };

        protected int selectedWeapon;
        protected Sprite selection;
        protected Texture selectionTexture;
        protected float itemWidth;
        //protected ProgressBar TimedBar;
        //public int SelectedWeapon
        //{
        //    get { return selectedWeapon; }
        //    protected set
        //    {
        //        selectedWeapon = value;
        //        //selection.position = weapons[selectedWeapon].Position;
        //    }
        //}

        public WeaponsGUI(Vector2 position, float w = 0, float h = 0) : base("WaponGui", DrawLayer.GUI, w, h)
        {
            sprite.pivot = Vector2.Zero;
            sprite.position = position;
            sprite.Camera = CameraMgr.GetCamera("GUI");
            sprite.scale *= 3;

            itemWidth = Game.PixelsToUnits(32);

            selectionTexture = GfxMgr.GetTexture("AngelFeather");
            selection = new Sprite(itemWidth, itemWidth);//selection has icon same size
            selection.scale *= 3;
            selection.Camera = CameraMgr.GetCamera("GUI");

            //TimedBar = new ProgressBar("barFrame", "blueBar",new Vector2(-0.03f,0));
            //TimedBar.IsActive = true;
            //TimedBar.Position = position+new Vector2(1.1f,0);

        }
        public override void Update()
        {
            //Console.WriteLine(TimedBar.Position);
            base.Update();

        }
        public override void Draw()
        {
            if (IsActive)
            {
                //Console.WriteLine("a gay");
                base.Draw();
                //selection.DrawTexture(selectionTexture);
                //TimedBar.Draw();
            }
            if (Inventory.Contains(PlayScene.AngelFeather))
            {
                selection.DrawTexture(selectionTexture);
                //Console.WriteLine("mi sto disegnando");
            }
        }
        
        //public BulletType DecrementsBullets()
        //{
        //    if (!weapons[selectedWeapon].IsInfinite)
        //    {
        //        weapons[selectedWeapon].DecremenBullets();

        //        if (!weapons[selectedWeapon].IsAvailable)
        //        {
        //           NextWeapon();
        //        }
        //    }

        //    return (BulletType)selectedWeapon;
        //}

        //public void AddBullets(BulletType type, int amount)
        //{
        //    weapons[(int)type].NumBullets += amount;
        //}
    }
}
