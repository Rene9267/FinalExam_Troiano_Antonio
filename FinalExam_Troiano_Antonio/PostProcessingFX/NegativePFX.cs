using System;
using Aiv.Fast2D;

namespace FinalExam_Troiano_Antonio
{
    class NegativePFX : PostProcessingEffect
    {
        private static string fragmentShader = @"
        #version 330 core
        out vec4 pixel_color;
        uniform sampler2D tex;
        in vec2 uv;
        
        uniform float NegativeValue;
        void main() {
            vec4 color = texture(tex, uv);
            pixel_color = vec4(color.r , color.g* NegativeValue, color.b* NegativeValue, 1.0);
        } 
        ";
        private float NegativeValue;
        private bool timeEnd;
        public NegativePFX() : base(fragmentShader)
        {
            NegativeValue = 1;
        }
        public override void Update(Window window)
        {
            if (!timeEnd)
            {
                NegativeValue -= Game.DeltaTime;
                if (NegativeValue <= 0) timeEnd = true;
            }
            else
            {
                NegativeValue += Game.DeltaTime;
                if (NegativeValue >= 1) timeEnd = false;
            }
            screenMesh.shader.SetUniform("NegativeValue", NegativeValue);
            base.Update(window);
        }
    }
}
