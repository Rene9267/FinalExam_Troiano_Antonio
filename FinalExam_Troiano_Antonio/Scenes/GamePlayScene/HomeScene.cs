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
    class HomeScene : PlayScene
    {
        public Key KeyOutdoor;
        public Items Carillon;
        private SoundEmitter mamaCarillon;
        private HavenScalePFX haven;
        public HomeScene() : base()
        {
        }
        public override void Start()
        {
            base.Start();
            LoadAssets();
           
            Carillon = new Items("Carillon", 5);
            KeyOutdoor = new Key(5);
            Keylist.Add(KeyOutdoor);

            CameraMgr.CameraLimits = new CameraLimits(8, 8.9f, 5f, 5f);

            #region MapTiled

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
                        cost = -1;
                    }
                    grid[row, col] = cost;
                }
            }
            float blockUnitWidth = Game.Window.OrthoWidth / grid.GetLength(1);//colonne
            float blockUnitHeight = Game.Window.OrthoHeight / grid.GetLength(0);//righe

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
                    doorNumber += 1;
                    tileObj.RigidBody = new RigidBody(tileObj);
                    tileObj.RigidBody.Type = RigidBodyType.TileDoor;
                    tileObj.RigidBody.Collider = ColliderFactory.CreateBoxFor(tileObj, 3);
                    tileObj.RigidBody.AddCollisionType(RigidBodyType.Player);
                    //tileObj.RigidBody.AddCollisionType(RigidBodyType.Enemy);
                    if (doorNumber <= 1)
                    {
                        WithDoor.Add(tileObj, Keylist[0]);
                    }
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
            player.Position = new Vector2(7.2f, 1.5f);
            player.IsActive = true;
            CameraMgr.SetTarget(player);

            Enemy = new Enemy(pathfinder, 6);
            Enemy.Position = new Vector2(13.7f, 6.3f);
            Enemy.IsActive = true;

            mamaCarillon = new SoundEmitter(Carillon, "CarillonFull");
            mamaCarillon.Play(true);
            haven = new HavenScalePFX();
            Game.Window.AddPostProcessingEffect(haven);
            KeyOutdoor.Position = Enemy.Position + new Vector2(1, 0);
            Carillon.Position = Enemy.Position + new Vector2(0, 1);
        }
        protected override void LoadAssets()
        {
            GfxMgr.AddTexture("GhostIdleSideWife", "Assets/GhostWife/IdleSprite/GhostWife_Idle_Side_HumanForm.gif");
            GfxMgr.AddTexture("Carillon", "Assets/Items/Carillon.png");
            GfxMgr.AddClip("CarillonFull", "Assets/AudioClip/MamaGhost/CarillonFull.wav");
            GfxMgr.AddClip("MamaScream", "Assets/AudioClip/MamaGhost/MamaScream.wav");
            base.LoadAssets();
        }
        public override void Update()
        {
            if (Game.DeltaTime > 0.1) return;
            haven.quality -= Game.DeltaTime;
            if (haven.quality <= 0) Game.Window.ClearPostProcessingEffects();
            if (Inventory.items.Contains(Carillon)) mamaCarillon.Volume = 0;
            base.Update();
        }
    }
}
