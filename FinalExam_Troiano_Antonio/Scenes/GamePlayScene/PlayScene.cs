using OpenTK;
using Aiv.Fast2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalExam_Troiano_Antonio;
using TiledPlugin;

namespace FinalExam_Troiano_Antonio
{
    class PlayScene : Scene
    {
        public static Map MiniMap;
        public static AngelFeather AngelFeather;

        public static List<Key> Keylist = new List<Key>();
        public static Dictionary<TileObj, Key> WithDoor = new Dictionary<TileObj, Key>();

        protected static float doorNumber = -1;

        public Enemy Enemy;
        public int[,] grid;
        public int[,] gridMama;
        public Player player;
        public List<TileObj> tiles;
        public bool IsMousePressed;
        public Vector2 MouseAttualPosition;

        protected bool PostEffect;
        protected Key KeyForestPlace;
        protected GrayScalePFX GrayCircle;
        protected GridPathfinder pathfinder;
        protected GridPathfinder pathfinderMama;
        protected Key KeyUndergroundInfernal;
        protected RandomTimer checkForVisiblePlayer;

        public PlayScene()
        {
        }
        public override void Start()
        {
            base.Start();
            LoadAssets();
            CameraMgr.Init();
            GrayCircle = new GrayScalePFX();
            CameraMgr.AddCamera("GUI", new Camera());
            KeyUndergroundInfernal = new Key();
        }
        protected virtual void LoadAssets()
        {
            //HeroSprite
            GfxMgr.AddTexture("HeroWalkFront", "Assets/Hero/Walk/Hero_Walk_Front.png");
            GfxMgr.AddTexture("HeroWalkSide", "Assets/Hero/Walk/Hero_Walk_Side.png");
            GfxMgr.AddTexture("HeroWalkBack", "Assets/Hero/Walk/Hero_Walk_Back.png");

            GfxMgr.AddTexture("HeroIdleFront", "Assets/Hero/Idle/Hero_Idle_Front.gif");
            GfxMgr.AddTexture("HeroIdleBack", "Assets/Hero/Idle/Hero_Idle_Back.gif");
            GfxMgr.AddTexture("HeroIdleSide", "Assets/Hero/Idle/Hero_Idle_Side.gif");

            //MamaGhostSprite
            GfxMgr.AddTexture("GhostIdleFront", "Assets/GhostWife/IdleSprite/GhostWife_Idle_Front.gif");
            GfxMgr.AddTexture("MamaRunFront", "Assets/GhostWife/FollowSprite/Mama_Run_Front.png");
            GfxMgr.AddTexture("MamaRunSide", "Assets/GhostWife/FollowSprite/Mama_Run_Side.png");
            GfxMgr.AddTexture("MamaRunBack", "Assets/GhostWife/FollowSprite/Mama_Run_Back.png");

            //ItemSprite
            GfxMgr.AddTexture("OutdoorKey", "Assets/Items/Key.png");
            GfxMgr.AddTexture("blueBar", "Assets/loadingBar_bar.png");
            GfxMgr.AddTexture("AngelShield", "Assets/AngelShield.png");
            GfxMgr.AddTexture("OpenedMap", "Assets/Items/MapOpen.png");
            GfxMgr.AddTexture("barFrame", "Assets/loadingBar_frame.png");
            GfxMgr.AddTexture("AngelFeather", "Assets/Items/Feather.png");
            GfxMgr.AddTexture("WaponGui", "Assets/weapon_GUI_selection.png");

            //ClipAudio
            GfxMgr.AddClip("WoodSteps", "Assets/AudioClip/Steps/WoodSteps.ogg");
            GfxMgr.AddClip("OpenMap", "Assets/AudioClip/Items/Map/OpenMap.wav");
            GfxMgr.AddClip("CloseMap", "Assets/AudioClip/Items/Map/CloseMap.wav");
            GfxMgr.AddClip("SandSteps", "Assets/AudioClip/Steps/SanStepsSound.wav");
            GfxMgr.AddClip("GrassSteps", "Assets/AudioClip/Steps/GrassStepsSound.wav");
            GfxMgr.AddClip("FindYou", "Assets/AudioClip/MamaGhost/I'mGonnaFindYou.wav");
            GfxMgr.AddClip("Can'tSeeMe", "Assets/AudioClip/MamaGhost/YuCan'tSeeMe.wav");
            GfxMgr.AddClip("AngelFeatherSound", "Assets/AudioClip/Items/FeatherSound.wav");
            GfxMgr.AddClip("AsphaltSteps", "Assets/AudioClip/Steps/AsphaltStepsSound.wav");
            GfxMgr.AddClip("DeathIs", "Assets/AudioClip/MamaGhost/DeathIsYourOnlyEscape.wav");
        }
        public override void Update()
        {
            PhysicsMgr.Update();
            UpdateMgr.Update();
            PhysicsMgr.CheckCollisions();
            CameraMgr.Update();

            if (Inventory.ContainsAll() && !(Inventory.items.Contains(KeyUndergroundInfernal)))
                Inventory.items.Add(KeyUndergroundInfernal);
            if (AngelFeather != null) AngelFeather.Update();
            if (MiniMap != null) MiniMap.Update();
        }
        public virtual void OnPlayerDies()
        {
            IsPlaying = false;
            NextScene = Game.gameOverScene;
        }
        public override void Draw()
        {
            DrawMgr.Draw();
            if (MiniMap != null) MiniMap.Draw();
            else Console.WriteLine("Nullo");
        }
        public override void Input()
        {
            player.Input();
        }
        public override Scene OnExit()
        {
            player = null;
            Enemy = null;
            UpdateMgr.ClearAll();
            DrawMgr.ClearAll();
            PhysicsMgr.ClearAll();
            GfxMgr.ClearAll();
            Game.Window.ClearPostProcessingEffects();
            //FontMgr.ClearAll();
            CameraMgr.ResetCamera();
            tiles.Clear();
            tiles = null;
            return base.OnExit();
        }
    }
}
