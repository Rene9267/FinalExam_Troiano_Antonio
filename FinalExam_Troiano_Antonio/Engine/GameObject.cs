using Aiv.Fast2D;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExam_Troiano_Antonio
{
    class GameObject : IUpdatable, IDrawable
    {
        protected Sprite sprite;
        public Texture texture;
        public Dictionary<ComponentType, Component> components;
        public RigidBody RigidBody;
        public virtual Vector2 Position
        {
            get { return sprite.position; }
            set { sprite.position = value; }
        }

        public float X { get { return sprite.position.X; } set { sprite.position.X = value; } }
        public float Y { get { return sprite.position.Y; } set { sprite.position.Y = value; } }

        public Sprite Sprite { get { return sprite; } }

        public Vector2 Forward
        {
            get
            {
                return new Vector2((float)Math.Cos(sprite.Rotation), (float)Math.Sin(sprite.Rotation));
            }
            set
            {
                sprite.Rotation = (float)Math.Atan2(value.Y, value.X);
            }
        }

        public float HalfWidth { get; protected set; }
        public float HalfHeight { get; protected set; }

        public DrawLayer Layer { get; protected set; }

        public bool IsActive;

        public virtual Camera Camera { get { return sprite.Camera; } set { sprite.Camera = value; } }

        public GameObject(string textureName, DrawLayer layer = DrawLayer.Playground, float w = 0, float h = 0)
        {
            texture = GfxMgr.GetTexture(textureName);
            sprite = new Sprite(w == 0 ? Game.PixelsToUnits(texture.Width) : w, h == 0 ? Game.PixelsToUnits(texture.Height) : h);
            sprite.pivot = new Vector2(sprite.Width * 0.5f, sprite.Height * 0.5f);

            HalfWidth = sprite.Width * 0.5f;
            HalfHeight = sprite.Height * 0.5f;

            Layer = layer;

            UpdateMgr.AddItem(this);
            DrawMgr.AddItem(this);

            components = new Dictionary<ComponentType, Component>();
        }

        public virtual void Update()
        {
        }
        public virtual void OnCollide(Collision collisionInfo)
        {
        }
        public virtual void Draw()
        {
            if (IsActive)
            {
                if (texture == null) {
                    Console.WriteLine(this.GetType()+"è zero");
                    return;
                }
                sprite.DrawTexture(texture);
            }
        }

        public Component GetComponent(ComponentType type)
        {
            if (components.ContainsKey(type))
            {
                return components[type];
            }
            return null;
        }

        public virtual void Destroy()
        {
            sprite = null;
            texture = null;

            UpdateMgr.RemoveItem(this);
            DrawMgr.RemoveItem(this);

            //rigidbody
            if (RigidBody != null)
            {
                RigidBody.Destroy();
                RigidBody = null;
            }
        }

        //~GameObject()
        //{
        //    Console.WriteLine("Distruttore chiamato su oggetto"+this.GetType());
        //}

    }
}
