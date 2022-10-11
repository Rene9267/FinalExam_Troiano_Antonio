using Aiv.Fast2D;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExam_Troiano_Antonio
{
    static class Game
    {
        private static List<Controller> controllers;
        private static KeyboardController keyboardController;

        public static Window Window;
        public static ForestScene forestScene;
        public static GameOverScene gameOverScene;
        public static Beatch_Room bigMap;
        public static EndScene EndScene;
        public static Scene CurrentScene { get; set; }
        public static Scene LastScene { get; set; }

        public static float DeltaTime { get { return Window.DeltaTime; } }
        public static float UnitSize { get; private set; }
        public static float OptimalScreenHeight { get; private set; }
        public static float OptimalUnitSize { get; private set; }
        public static Vector2 ScreenCenter { get; private set; }
        public static float ScreenDiagonalSquared { get; private set; }
        public static GrayScalePFX Gray { get; private set; }
        public static void Init()
        {
            Window = new Window(1280, 720, "FinalProgram");
            Window.SetVSync(false);
            Window.SetDefaultViewportOrthographicSize(10);

            OptimalScreenHeight = 1080;

            UnitSize = Window.Height / Window.OrthoHeight;
            OptimalUnitSize = OptimalScreenHeight / Window.OrthoHeight;//108

            ScreenCenter = new Vector2(Window.OrthoWidth * 0.5f, Window.OrthoHeight * 0.5f);
            ScreenDiagonalSquared = ScreenCenter.LengthSquared;

            //Gray = new GrayScalePFX();
            //Game.Window.AddPostProcessingEffect(Gray);

            TitleScene titleScene = new TitleScene("Assets/Levels/aivBG.png");
            gameOverScene = new GameOverScene("Assets/Levels/aivBG.png");
            //PlayScene playScene = new PlayScene();
            HomeScene homeScene = new HomeScene();
            GoodEndScene goodEndScene = new GoodEndScene();
            bigMap = new Beatch_Room();
            forestScene = new ForestScene();
            EndScene = new EndScene();

            EndScene.NextScene = gameOverScene;
            titleScene.NextScene = homeScene;
            homeScene.NextScene = bigMap;
            bigMap.NextScene = forestScene;
            forestScene.NextScene = bigMap;
            EndScene.NextScene = gameOverScene;
            //playScene.NextScene = gameOverScene;
            gameOverScene.NextScene = LastScene;

            List<KeyCode> keys = new List<KeyCode>();
            keys.Add(KeyCode.W);
            keys.Add(KeyCode.S);
            keys.Add(KeyCode.D);
            keys.Add(KeyCode.A);
            keys.Add(KeyCode.Space);

            KeysList keyList = new KeysList(keys);
            keyboardController = new KeyboardController(0, keyList);

            string[] joystics = Window.Joysticks;
            controllers = new List<Controller>();

            for (int i = 0; i < joystics.Length; i++)
            {
                if (joystics[i] != null && joystics[i] != "Unmapped Controller")
                {
                    controllers.Add(new JoypadController(i));
                }
            }
            CurrentScene = titleScene;
        }

        public static float PixelsToUnits(float pizelsSize)
        {
            return pizelsSize / OptimalUnitSize;
        }

        public static Controller GetController(int index)
        {
            Controller ctrl = keyboardController;

            if (index < controllers.Count)
            {
                ctrl = controllers[index];
            }
            return ctrl;
        }

        public static void Play()
        {
            //Counter myCounter = new Counter();

            CurrentScene.Start();

            while (Window.IsOpened)
            {
                //float fps = 1f / Window.DeltaTime;
                //Window.SetTitle($"FPS: {fps}");

                if (!CurrentScene.IsPlaying)
                {
                    Scene nextScene = CurrentScene.OnExit();
                    GC.Collect();//explicit call to Garbage Collector

                    if (nextScene != null)
                    {
                        LastScene = CurrentScene;
                        CurrentScene = nextScene;
                        CurrentScene.Start();
                    }
                    else
                    {
                        return;
                    }
                }

                //INPUT
                if (Window.GetKey(KeyCode.Esc))
                {
                    break;
                }
                CurrentScene.Input();

                //UPDATE
                CurrentScene.Update();

                //DRAW
                CurrentScene.Draw();

                Window.Update();
            }
        }
    }


}
