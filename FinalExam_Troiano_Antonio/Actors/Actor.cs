using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace FinalExam_Troiano_Antonio
{
    abstract class Actor : GameObject
    {
        public Animation animation;
        protected Actor(string textureName, DrawLayer layer = DrawLayer.Playground, float w = 0, float h = 0)
            : base(textureName, layer, w, h)
        {
            float unitDist = Game.PixelsToUnits(4);
            animation = new Animation(this, 4, 16, 16, 10);
            animation.IsEnabled = false;
            components.Add(ComponentType.Animation, animation);
            animation.Play();
        }
        public override void OnCollide(Collision collisionInfo)
        {
            OnWallCollides(collisionInfo);
        }
        protected virtual void OnWallCollides(Collision collisionInfo)
        {
            if (collisionInfo.Delta.X < collisionInfo.Delta.Y)
            {
                //horizontal collision
                if (X < collisionInfo.Collider.X)
                {
                    //collision from left
                    collisionInfo.Delta.X = -collisionInfo.Delta.X;
                }
                X += collisionInfo.Delta.X;
                RigidBody.Velocity.X = 0;
            }
            else
            {
                //vertical collision
                if (Y < collisionInfo.Collider.Y)
                {
                    //collision from top
                    collisionInfo.Delta.Y = -collisionInfo.Delta.Y;
                    RigidBody.Velocity.Y = 0;
                }
                else
                {
                    //collision from bottom
                    RigidBody.Velocity.Y = -RigidBody.Velocity.Y * 0.8f;
                }
                Y += collisionInfo.Delta.Y;
            }
        }
        public abstract void OnDie();
    }
}
