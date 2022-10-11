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
    class EndScene : PlayScene
    {
        public SoundEmitter HellSound;
        public EndScene() { }
        public override void Start()
        {
            base.Start();
            LoadAssets();
            WobblePFX wobblePFX = new WobblePFX();
            NegativePFX negativePFX = new NegativePFX();
            Game.Window.AddPostProcessingEffect(negativePFX);
            Game.Window.AddPostProcessingEffect(wobblePFX);
            CameraMgr.CameraLimits = new CameraLimits(8, 8.9f, 5f, 5f);

            #region MapRender
            TmxReader reader = new TmxReader("Assets/Levels/Home_Room.tmx");
            TmxTileset tileset = reader.TileSet;
            TmxGrid tmxGrid = reader.TileLayers[0].Grid;
            grid = new int[tmxGrid.Rows, tmxGrid.Cols];
            for (int row = 0; row < tmxGrid.Rows; row++)
            {
                for (int col = 0; col < tmxGrid.Cols; col++)
                {
                    int index = row * tmxGrid.Cols + col;
                    TmxCell cell = tmxGrid.At(index);
                    int cost = 1;
                    if (cell != null && cell.Type.Props.Has("cost"))
                    {
                        cost = cell.Type.Props.GetInt("cost");
                    }
                    if (cell != null && cell.Type.Props.Has("Collidable") && cell.Type.Props.GetBool("Collidable"))
                    {
                        cost = 200;
                    }
                    grid[row, col] = cost;
                }
            }
            float blockUnitWidth = Game.Window.OrthoWidth / grid.GetLength(1);
            float blockUnitHeight = Game.Window.OrthoHeight / grid.GetLength(0);
            pathfinder = new GridPathfinder(grid, blockUnitWidth, blockUnitHeight);
            // Map Rendering
            GfxMgr.AddTexture("tileset", "Assets/Tiled/" + tileset.TilesetPath);
            tiles = new List<TileObj>();
            int size = tmxGrid.Rows * tmxGrid.Cols;
            for (int index = 0; index < size; index++)
            {
                //if (grid[row,col] == 200)
                TmxCell cell = tmxGrid.At(index);
                if (cell == null) continue;
                float pixelToUnitW = blockUnitWidth / cell.Type.Width;
                float pixelToUnitH = blockUnitHeight / cell.Type.Height;

                float posX = cell.PosX * pixelToUnitW + blockUnitWidth / 2;
                float posY = cell.PosY * pixelToUnitH + blockUnitHeight / 2;
                TileObj tileObj = new TileObj("tileset",
                    cell.Type.OffX, cell.Type.OffY,
                    cell.Type.Width, cell.Type.Height,
                    posX, posY,
                    blockUnitWidth, blockUnitHeight
                );
                tiles.Add(tileObj);

                if (cell.Type.Props.Has("Door"))
                {
                    tileObj.RigidBody = new RigidBody(tileObj);
                    tileObj.RigidBody.Type = RigidBodyType.TileDoor;
                    tileObj.RigidBody.Collider = ColliderFactory.CreateBoxFor(tileObj, 3);
                    tileObj.RigidBody.AddCollisionType(RigidBodyType.Player);
                    tileObj.RigidBody.AddCollisionType(RigidBodyType.Enemy);
                }
                if (cell.Type.Props.Has("Sand"))
                    tileObj.terrain = Terrain.Sand;
                if (cell.Type.Props.Has("Grass"))
                    tileObj.terrain = Terrain.Grass;
                if (cell.Type.Props.Has("Asphalt"))
                    tileObj.terrain = Terrain.Asphalt;
                if (cell.Type.Props.Has("Wood"))
                    tileObj.terrain = Terrain.Wood;
            }
            #endregion

            player = new Player(Game.GetController(0), pathfinder, 6);
            player.Position = new Vector2(9, 7);
            player.IsActive = true;
            CameraMgr.SetTarget(player);

            Enemy = new Enemy(pathfinder, 6);
            Enemy.Position = new Vector2(8.8f, 1.7f);
            Enemy.IsActive = true;
            SoundEmitter Frase = new SoundEmitter(Enemy, "WelcomeToHell");
            HellSound = new SoundEmitter(Enemy, "HellSound");
            HellSound.Play(0.3f);
            Frase.Play(1,0.8f);
        }
        protected override void LoadAssets()
        {
            GfxMgr.AddTexture("HumanMamaDown","Assets/GhostWife/HumanForm/Human_Mama_Walk_Down.png");
            GfxMgr.AddTexture("HumanMamaUp", "Assets/GhostWife/HumanForm/Human_Mama_Walk_Up.png");
            GfxMgr.AddTexture("HumanMamaSide", "Assets/GhostWife/HumanForm/Human_Mama_Walk_Side.png");
            GfxMgr.AddTexture("HumanMamaHug", "Assets/GhostWife/HumanForm/Human_Mama_Hug.gif");
            GfxMgr.AddTexture("KidHug", "Assets/Hero/Idle/Hero_Idle_Hug.gif");

            GfxMgr.AddClip("HellSound", "Assets/AudioClip/HellSound.wav");
            GfxMgr.AddClip("WelcomeToHell","Assets/AudioClip/MamaGhost/WelcomeToHell.wav");
            //GfxMgr.AddClip("Run");
            base.LoadAssets();
        }
        public override void Update()
        {
            base.Update();
        }
        public override Scene OnExit()
        {
            HellSound = null;
            return base.OnExit();
        }
    }
}
