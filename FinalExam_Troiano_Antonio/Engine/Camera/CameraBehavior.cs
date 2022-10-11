using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace FinalExam_Troiano_Antonio
{
    abstract class CameraBehavior
    {
        protected Camera camera;
        protected Vector2 pointToFollow;
        protected float blendFactor;

        public CameraBehavior(Camera camera)
        {
            this.camera = camera;
        }

        public virtual void Update()
        {
            camera.position = Vector2.Lerp(camera.position, pointToFollow, blendFactor);
        }
    }
}
