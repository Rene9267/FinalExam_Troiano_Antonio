using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace FinalExam_Troiano_Antonio
{
    class HavenScalePFX : PostProcessingEffect
    {
        private static string fragmentShader = @"
        #version 330 core
        out vec4 pixel_color;
        
        uniform sampler2D tex;
        in vec2 uv;

        uniform float winW;
        uniform float winH;
        uniform float quality;
        const float PI = 3.14;
        
        void main() {
            vec4 color = texture(tex, uv);
            
            float perimeter = 2*PI; 
            float directions = 8;
            float dirStep = perimeter/directions;
            
            float unitaryRay = 1.0f;
            float qualityStep = unitaryRay / quality;
        
            float radius = 6;
            vec2 size = vec2(winW, winH);
            vec2 strength = radius / size;

            for (float dir = 0.0; dir < perimeter ; dir += dirStep) {
                for(float qua = qualityStep; qua < unitaryRay; qua+= qualityStep) {
                    vec2 uvCurrent = uv + vec2(cos(dir), sin(dir)) * strength * qua;
                    color += texture(tex, uvCurrent);
                }
            }
            color /= (quality * directions) + 1.0;

            pixel_color = color;
        } 
        ";
        public float quality { get; set; }
        public HavenScalePFX() : base(fragmentShader)
        {
            quality = 1;
        }
        public override void Update(Window window)
        {
            screenMesh.shader.SetUniform("quality", quality);
            screenMesh.shader.SetUniform("winW", (float)window.Width);
            screenMesh.shader.SetUniform("winH", (float)window.Height);
            base.Update(window);
        }
    }
}
