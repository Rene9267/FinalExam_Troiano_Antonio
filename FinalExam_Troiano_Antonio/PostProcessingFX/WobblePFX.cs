using System;
using Aiv.Fast2D;

namespace FinalExam_Troiano_Antonio
{
    class WobblePFX : PostProcessingEffect
    {
        private static string fragmentShader = @"
        #version 330 core
        out vec4 pixel_color;
        
        uniform sampler2D tex;
        in vec2 uv;

        uniform float timeSpeed;
        
        void main() {
            vec2 uvFinal = uv;
            
            uvFinal.x += sin( uvFinal.y * 3*3*3.14159 + timeSpeed) / 100;
            
            pixel_color = texture(tex, uvFinal);
        } 
        ";
        private float timeSpeed;

        public WobblePFX() : base(fragmentShader) {
            timeSpeed = 0f;
        }

        public override void Update(Window window)
        {
            timeSpeed += window.DeltaTime ;
            screenMesh.shader.SetUniform("timeSpeed", timeSpeed);
        }
    }
}
