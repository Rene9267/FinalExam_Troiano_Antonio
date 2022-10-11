using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using FinalExam_Troiano_Antonio;


namespace TiledPlugin
{
    enum Terrain { Sand, Wood, Grass, Asphalt }
    class TileObj : GameObject
    {
        private int texOffX;
        private int texOffY;
        private int texWidth;
        private int texHeight;
        public Terrain terrain;
        public bool EndDoor;
        //public bool Sand;
        //public bool Wood;
        //public bool Grass;
        //public bool Asphalt;

        public TileObj(string texture,
           int tOffX, int tOffY,
           int tWidth, int tHeight,

           float posX, float posY,
           float width, float height)
            : base(texture, DrawLayer.Background, width, height)
        {
            //sprite = new Sprite(width, height);
            sprite.position.X = posX;
            sprite.position.Y = posY;
            texOffX = tOffX;
            texOffY = tOffY;
            texWidth = tWidth;
            texHeight = tHeight;

            IsActive = true;
        }
        public override void OnCollide(Collision collisionInfo)
        {
            if (collisionInfo.Collider is Player && RigidBody.Type == RigidBodyType.TileDoor)
            {
                if (Inventory.items == null) return;
                if (this.EndDoor && Inventory.InventoryCount == 6)
                {
                    Game.bigMap.NextScene = Game.EndScene;
                    Game.CurrentScene.IsPlaying = false;
                }

                if (Game.CurrentScene is ForestScene || Game.CurrentScene is EndScene) Game.CurrentScene.IsPlaying = false;
                else if (Inventory.Contains(PlayScene.WithDoor[this])) Game.CurrentScene.IsPlaying = false;
                else return;
            }
        }
        public override void Draw()
        {
            if (!IsActive) return;
            sprite.DrawTexture(texture, texOffX, texOffY, texWidth, texHeight);
        }
    }
}
