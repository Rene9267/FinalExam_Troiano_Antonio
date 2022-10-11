using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExam_Troiano_Antonio
{
    class Items : GameObject
    {
        public Items(string textureName, float scale = 0, DrawLayer layer = DrawLayer.Playground, float w = 0, float h = 0) : base(textureName, layer, w, h)
        {
            //if (!Inventory.Contains(this))
            //{
            IsActive = false;
            RigidBody = new RigidBody(this);
            RigidBody.Type = RigidBodyType.Items;
            RigidBody.AddCollisionType(RigidBodyType.Player);
            RigidBody.Collider = ColliderFactory.CreateBoxFor(this, scale);
            sprite.scale *= scale;
            //}
        }

        public override void OnCollide(Collision collisionInfo)
        {
            if (collisionInfo.Collider is Player)
            {
                if (Game.Window.GetKey(Aiv.Fast2D.KeyCode.F))
                {
                    if (this.GetType() == typeof(Items)) Inventory.InventoryCount += 1;
                    if (!(Inventory.items.Contains(this)))
                    {
                        Inventory.items.Add(this);
                        IsActive = false;
                    }
                }
            }
        }
    }
}
