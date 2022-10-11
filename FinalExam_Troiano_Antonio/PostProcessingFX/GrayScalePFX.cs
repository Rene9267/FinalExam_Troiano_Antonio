using System;
using Aiv.Fast2D;
using OpenTK;

namespace FinalExam_Troiano_Antonio
{
    class GrayScalePFX : PostProcessingEffect
    {
        private static string fragmentShader = @"
        #version 330 core
        out vec4 pixel_color;
        uniform sampler2D tex;
        in vec2 uv;
           
            uniform float pointPosX;
            uniform float pointPosY;
            uniform float screenWidth;
            uniform float screenHeight;

            uniform float aRadius;

            void main()
            {
                vec2 pointPos=vec2 (pointPosX/screenWidth,1-(pointPosY/screenHeight));

                float dist = distance(pointPos, uv);
                if (dist > aRadius)
                    discard;

                vec4 aColor= texture (tex, uv);
                float d = dist / aRadius;

                pixel_color = vec4(aColor.r-d,aColor.g-d,aColor.b-d, 1.0);
        }";
        float timeSpeed;
        float dist;
        public GrayScalePFX() : base(fragmentShader)
        {
            timeSpeed = 0;
            //Console.WriteLine(CameraMgr.MainCamera.pivot);
        }
        public override void Update(Window window)
        {
            Vector2 vector2 = ((PlayScene)Game.CurrentScene).player.Position;
            Vector2 enemyPos = ((PlayScene)Game.CurrentScene).Enemy.Position;
            Vector2 playerRelativePosition = vector2 - CameraMgr.MainCamera.position + CameraMgr.MainCamera.pivot;
            //Vector2 playerRelativePosition = ;
           
            dist = (vector2 - enemyPos).Length;
            dist /= 8; //per normalizzare(più lo aumento, più graduale è il cambio colore)
            
            screenMesh.shader.SetUniform("screenWidth", window.OrthoWidth);
            screenMesh.shader.SetUniform("screenHeight", window.OrthoHeight);
            screenMesh.shader.SetUniform("pointPosX", /*CameraMgr.MainCamera.pivot.X*/playerRelativePosition.X);
            screenMesh.shader.SetUniform("pointPosY",/* CameraMgr.MainCamera.pivot.Y*/playerRelativePosition.Y);
            screenMesh.shader.SetUniform("aRadius", dist);
            base.Update(window);
        }
    }
}