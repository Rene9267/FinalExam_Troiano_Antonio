using Aiv.Fast2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace FinalExam_Troiano_Antonio
{
    class ProgressBar : GameObject
    {
        protected Sprite barSprite;
        protected Texture barTexture;

        protected Vector2 barOffset;
        protected int barWidth;
        private float time=5;

        public override Vector2 Position { get => base.Position; set { base.Position = value; barSprite.position = value + barOffset; } }

        public override Camera Camera { get => base.Camera; set { base.Camera = value; barSprite.Camera = value; } }

        public ProgressBar(string frameTextureName, string barTextureName, Vector2 innerBarOffset) : base(frameTextureName, DrawLayer.GUI)
        {
            sprite.pivot = Vector2.Zero;
            IsActive = true;

            barOffset = innerBarOffset;
            barTexture = GfxMgr.GetTexture(barTextureName);

            barSprite = new Sprite(Game.PixelsToUnits(barTexture.Width), Game.PixelsToUnits(barTexture.Height));
            barWidth = (int)barTexture.Width;

            barSprite.scale.X *= 0.5f;
            Sprite.scale.X *= 0.5f;

            barSprite.Rotation = 1.58f;
            sprite.Rotation = 1.58f;

            //barSprite.FlipX = true;
            
            Camera = CameraMgr.GetCamera("GUI");
        }
        public virtual void Scale(float scale)
        {
            scale = MathHelper.Clamp(scale, 0, 1);

            barSprite.scale.X = scale;
            barWidth = (int)(barTexture.Width * scale);

            barSprite.SetMultiplyTint((1 - scale) * 50, scale * 2, scale, 1);
        }
        public override void Update()
        {
            base.Update();
            time -= Game.DeltaTime;
            Scale(time/5);
        }
        public override void Draw()
        {
            if (IsActive)
            {
                base.Draw();
                barSprite.DrawTexture(barTexture, 0, 0, barWidth, (int)barTexture.Height);

            }
        }
    }
}
