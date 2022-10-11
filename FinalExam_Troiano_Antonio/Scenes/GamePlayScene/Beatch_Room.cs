using OpenTK;
using Aiv.Fast2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledPlugin;


namespace FinalExam_Troiano_Antonio
{

    class Beatch_Room : PlayScene
    {
        public Items Shell;
        public Items Shovel;

        public Beatch_Room()
        {
        }
        public override void Start()
        {
            base.Start();
            LoadAssets();
            #region Items Initiation
            if (!Inventory.Contains(Shell))
            {
                Shell = new Items("Shell", 3);
                Shell.Position = new Vector2(9, 18);
                Shell.IsActive = true;
            }
            if (!Inventory.Contains(Shovel))
            {
                Shovel = new Items("Shovel", 3);
                Shovel.Position = new Vector2(9, 24);
                Shovel.IsActive = true;
            }
            if (!Inventory.Contains(MiniMap))
            {
                MiniMap = new Map(3);
                MiniMap.Position = new Vector2(34.5f, 20.5f);
                MiniMap.IsActive = true;
            }
            if (!Inventory.Contains(KeyForestPlace))
            {
                KeyForestPlace = new Key(3);
                KeyForestPlace.IsActive = true;
                Keylist.Add(KeyForestPlace);
                KeyForestPlace.Position = new Vector2(44.5f, 24);
            }
            if (!Inventory.Contains(AngelFeather))
            {
                AngelFeather = new AngelFeather(3);
                AngelFeather.Position = new Vector2(33.5f, 20.5f);
                AngelFeather.IsActive = true;
            }
            #endregion

            CameraMgr.CameraLimits = new CameraLimits(Game.Window.OrthoWidth * 2.5f, Game.Window.OrthoWidth * 0.5f, Game.Window.OrthoHeight * 2.5f, 1);

            #region Map Rendering
            TmxReader reader = new TmxReader("Assets/Levels/Beatch_RooM.tmx");
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
            gridMama = new int[tmxGrid.Rows, tmxGrid.Cols];
            for (int row = 0; row < tmxGrid.Rows; row++)
            {
                for (int col = 0; col < tmxGrid.Cols; col++)
                {
                    int index = row * tmxGrid.Cols + col;
                    TmxCell cell = tmxGrid.At(index);
                    int cost = 1;
                    gridMama[row, col] = cost;
                }
            }
            float blockUnitWidth = Game.Window.OrthoWidth / grid.GetLength(1) * 3;
            float blockUnitHeight = Game.Window.OrthoHeight / grid.GetLength(0) * 3;
            pathfinder = new GridPathfinder(grid, blockUnitWidth, blockUnitHeight);
            pathfinderMama = new GridPathfinder(gridMama, blockUnitWidth, blockUnitHeight);
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
                    if (doorNumber >= 5)
                    {
                        doorNumber = 1;
                    }
                    doorNumber += 1;
                    tileObj.RigidBody = new RigidBody(tileObj);
                    tileObj.RigidBody.Type = RigidBodyType.TileDoor;
                    tileObj.RigidBody.Collider = ColliderFactory.CreateBoxFor(tileObj, 3);
                    tileObj.RigidBody.AddCollisionType(RigidBodyType.Player);
                    tileObj.RigidBody.AddCollisionType(RigidBodyType.Enemy);
                    if (doorNumber <= 1)
                    {
                        //if(!WithDoor.ContainsValue(Keylist[0]))
                        WithDoor.Add(tileObj, Keylist[0]);
                    }
                    else if (doorNumber <= 3 && doorNumber >= 2)
                    {
                        //if (!WithDoor.ContainsValue(Keylist[1]))
                        WithDoor.Add(tileObj, KeyUndergroundInfernal);
                        tileObj.EndDoor = true;
                    }
                    else if (doorNumber <= 5 && doorNumber >= 4)
                    {
                        //if (!WithDoor.ContainsValue(Keylist[2]))
                        WithDoor.Add(tileObj, Keylist[1]);
                    }
                }
                if (cell.Type.Props.Has("Sand"))
                {
                    tileObj.terrain = Terrain.Sand;
                }
                if (cell.Type.Props.Has("Grass"))
                {
                    tileObj.terrain = Terrain.Grass;
                }
                if (cell.Type.Props.Has("Asphalt"))
                {
                    tileObj.terrain = Terrain.Asphalt;
                }
                if (cell.Type.Props.Has("Wood"))
                {
                    tileObj.terrain = Terrain.Wood;
                }
            }
            #endregion

            #region Actors
            player = new Player(Game.GetController(0), pathfinder, 4.5f);
            player.IsActive = true;
            if ((Game.LastScene is ForestScene))
            {
                player.Position = new Vector2(37.5f, 15.5f);
            }
            else
                player.Position = new Vector2(36.5f, 19.5f);

            CameraMgr.SetTarget(player);

            Enemy = new Enemy(pathfinderMama, 4.5f);
            SoundEmitter Frase = new SoundEmitter(Enemy, "ThingsInTheShadow");
            if (Game.LastScene is HomeScene) Frase.Play();
            #endregion
        }
        protected override void LoadAssets()
        {
            GfxMgr.AddTexture("Shell", "Assets/Items/Shell.png");
            GfxMgr.AddTexture("Shovel", "Assets/Items/Shovel.png");
            GfxMgr.AddTexture("Map", "Assets/Items/Maps.png");

            GfxMgr.AddClip("ThingsInTheShadow", "Assets/AudioClip/MamaGhost/Things_In_The_Shadow.wav");
            base.LoadAssets();
        }
        public override void Update()
        {
            if (Enemy.IsActive)
            {
                if (!PostEffect)
                {
                    Game.Window.AddPostProcessingEffect(GrayCircle);
                    PostEffect = true;
                }
            }
            else
            {
                Game.Window.ClearPostProcessingEffects();
                PostEffect = false;
            }
            base.Update();
        }

    }
}
