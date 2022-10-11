using Aiv.Fast2D;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExam_Troiano_Antonio
{
    struct CameraLimits
    {
        public float MaxX;
        public float MinX;
        public float MaxY;
        public float MinY;

        public CameraLimits(float maxX, float minX, float maxY, float minY)
        {
            MaxX = maxX;
            MinX = minX;
            MaxY = maxY;
            MinY = minY;
        }
    }

    enum CameraBehaviorType { FollowTarget, FollowPoint, MoveToPoint, LAST }

    static class CameraMgr
    {
        private static Dictionary<string, Tuple<Camera, float>> cameras;

        public static Camera MainCamera;

        public static float CameraSpeed = 5;

        public static CameraLimits CameraLimits;

        public static float HalfDiagonalSquared { get; private set; }

        private static CameraBehavior[] behaviors;
        private static CameraBehavior currentBehavior;

        public static void Init()
        {
            if (Game.Window.CurrentCamera == null)
                MainCamera = new Camera(Game.Window.OrthoWidth * 0.5f, Game.Window.OrthoHeight * 0.5f);
            else
                MainCamera = Game.Window.CurrentCamera;

            MainCamera.pivot = new Vector2(Game.Window.OrthoWidth * 0.5f, Game.Window.OrthoHeight * 0.5f);

            HalfDiagonalSquared = MainCamera.pivot.LengthSquared;

            cameras = new Dictionary<string, Tuple<Camera, float>>();

            behaviors = new CameraBehavior[(int)CameraBehaviorType.LAST];

            behaviors[(int)CameraBehaviorType.FollowTarget] = new FollowTargetBehavior(MainCamera, null);
            behaviors[(int)CameraBehaviorType.FollowPoint] = new FollowPointBehavior(MainCamera, Vector2.Zero);
            behaviors[(int)CameraBehaviorType.MoveToPoint] = new MoveToPointBehavior(MainCamera);
            currentBehavior = behaviors[0];
        }

        public static void SetTarget(GameObject target, bool changeBehavior = true)
        {
            FollowTargetBehavior followTarget = (FollowTargetBehavior)behaviors[(int)CameraBehaviorType.FollowTarget];
            followTarget.Target = target;

            if (changeBehavior)
            {
                currentBehavior = followTarget;
            }
        }

        public static void SetPointToFollow(Vector2 point)
        {
            //set point to FollowPointBehavior
            currentBehavior = behaviors[(int)CameraBehaviorType.FollowPoint];
            ((FollowPointBehavior)currentBehavior).SetPointToFollow(point);
        }

        public static void MoveTo(Vector2 point, float time)
        {
            //set point and time to MoveToPointBehavior
            currentBehavior = behaviors[(int)CameraBehaviorType.MoveToPoint];
            ((MoveToPointBehavior)currentBehavior).MoveTo(point, time);
        }

        public static void ResetCamera()
        {
            MainCamera.position = Vector2.Zero;
            MainCamera.pivot = Vector2.Zero;
            behaviors = null;

            cameras.Clear();
        }

        public static void AddCamera(string cameraName, Camera camera = null, float cameraSpeed = 0)
        {
            if (camera == null)
            {
                camera = new Camera(MainCamera.position.X, MainCamera.position.Y);
                camera.pivot = MainCamera.pivot;
            }

            cameras[cameraName] = new Tuple<Camera, float>(camera, cameraSpeed);
        }

        public static Camera GetCamera(string cameraName)
        {
            if (cameras.ContainsKey(cameraName))
            {
                return cameras[cameraName].Item1;
            }
            return null;
        }

        public static void Update()
        {
            Vector2 oldCameraPos = MainCamera.position;
            currentBehavior.Update();
            FixPosition();

            Vector2 cameraDelta = MainCamera.position - oldCameraPos;
            UpdateCameras(cameraDelta);
        }

        private static void UpdateCameras(Vector2 cameraDelta)
        {
            if (cameraDelta != Vector2.Zero)
            {
                foreach (var item in cameras)
                {
                    item.Value.Item1.position += cameraDelta * item.Value.Item2;//camera.position += delta * cameraSpeed
                }
            }
        }

        private static void FixPosition()
        {
            MainCamera.position.X = MathHelper.Clamp(MainCamera.position.X, CameraLimits.MinX, CameraLimits.MaxX);
            MainCamera.position.Y = MathHelper.Clamp(MainCamera.position.Y, CameraLimits.MinY, CameraLimits.MaxY);
        }

        public static void OnMovementEnd()
        {
            currentBehavior = behaviors[(int)CameraBehaviorType.FollowTarget];
        }

    }
}
