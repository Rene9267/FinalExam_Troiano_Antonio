using Aiv.Fast2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExam_Troiano_Antonio
{
    class FollowTargetBehavior : CameraBehavior
    {
        public GameObject Target;
        public static float CameraSpeed = 5;

        public FollowTargetBehavior(Camera camera, GameObject target) : base(camera)
        {
            Target = target;
        }

        public override void Update()
        {
            if (Target != null)
            {
                pointToFollow = Target.Position;
                blendFactor = Game.DeltaTime * CameraSpeed;

                base.Update();
            }
        }
    }
}
