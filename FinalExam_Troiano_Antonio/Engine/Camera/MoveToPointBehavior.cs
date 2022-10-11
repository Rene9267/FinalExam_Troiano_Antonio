using Aiv.Fast2D;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExam_Troiano_Antonio
{
    class MoveToPointBehavior : CameraBehavior
    {
        protected float counter;
        protected float duration;
        protected Vector2 cameraStartPosition;

        public MoveToPointBehavior(Camera camera) : base(camera)
        {
        }

        public virtual void MoveTo(Vector2 point, float movementDuration)
        {
            cameraStartPosition = camera.position;
            pointToFollow = point;
            duration = movementDuration;
            counter = 0;
            blendFactor = 0;
        }

        public override void Update()
        {
            counter += Game.DeltaTime;

            if (counter >= duration)
            {
                counter = duration;
                CameraMgr.OnMovementEnd();
            }

            blendFactor = counter / duration;

            camera.position = Vector2.Lerp(cameraStartPosition, pointToFollow, blendFactor);
        }
    }
}
