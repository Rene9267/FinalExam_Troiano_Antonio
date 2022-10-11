using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExam_Troiano_Antonio
{
    enum RigidBodyType { Player=1, Tile = 2, Enemy=4, TileDoor=8, Items=16 }

    class RigidBody
    {
        protected uint collisionMask;

        public Vector2 Velocity;
        public GameObject GameObject;
        public bool IsGravityAffected;
        public bool IsCollisionsAffected = true;

        public float Friction;

        public RigidBodyType Type;
        public Collider Collider;
        

        public bool IsActive { get { return GameObject.IsActive; } set { GameObject.IsActive = value; } }
        public Vector2 Position { get { return GameObject.Position; } }

        public RigidBody(GameObject owner)
        {
            GameObject = owner;
            PhysicsMgr.AddItem(this);
        }

        //Collider

        public void Update()
        {
            //gravity
            if (IsGravityAffected)
            {
                Velocity.Y += PhysicsMgr.G *Game.DeltaTime;
            }

            ApplyFriction();

            GameObject.Position += Velocity * Game.DeltaTime;
        }

        protected void ApplyFriction()
        {
            if(Friction!=0 && Velocity != Vector2.Zero)
            {
                float fAmount = Friction * Game.DeltaTime;
                float newVelocityLength = Velocity.Length - fAmount;

                if(newVelocityLength< 0)
                {
                    newVelocityLength = 0;
                }

                Velocity = Velocity.Normalized() * newVelocityLength;
            }
        }

        public void AddCollisionType(RigidBodyType type)
        {
            //collisionMask = collisionMask | (uint)type;
            collisionMask |= (uint)type;
        }

        public void AddCollisionType(uint value)
        {
            collisionMask |= value;
        }

        public bool CollisionTypeMatches(RigidBodyType type)
        {
            return ((uint)type & collisionMask) != 0;
        }

        public bool Collides(RigidBody other, ref Collision collisionInfo)
        {
            return Collider.Collides(other.Collider, ref collisionInfo);
        }

        public virtual void Destroy()
        {
            GameObject = null;
            Collider = null;
            PhysicsMgr.RemoveItem(this);
        }
    }
}
