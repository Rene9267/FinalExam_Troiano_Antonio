using Aiv.Fast2D;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExam_Troiano_Antonio
{
    class FollowPointBehavior : CameraBehavior
    {
        public static float CameraSpeed = 5;

        public FollowPointBehavior(Camera camera, Vector2 point) : base(camera)
        {
        }

        public virtual void SetPointToFollow(Vector2 point)
        {
            pointToFollow = point;
        }

        public override void Update()
        {
            blendFactor = Game.DeltaTime * CameraSpeed;
            base.Update();
        }
    }
}
