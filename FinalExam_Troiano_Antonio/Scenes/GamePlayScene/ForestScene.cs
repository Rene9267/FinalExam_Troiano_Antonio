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
    class ForestScene : PlayScene
    {
        private Items Pokeball;
        private Items Mushroom;
        private Items Leaf;

        public ForestScene()
        {
        }
        public override void Start()
        {
            LoadAssets();
            base.Start();
            if (!Inventory.Contains(Pokeball))
            {
                Pokeball = new Items("Pokeball", 3);
                Pokeball.Position = new Vector2(12.7f, 6.5f);
                Pokeball.IsActive = true;
            }
            if (!Inventory.Contains(Mushroom))
            {
                Mushroom = new Items("Mushroom", 3);
                Mushroom.Position = new Vector2(26.1f, 27);
                Mushroom.IsActive = true;
            }
            if (!Inventory.Contains(Leaf))
            {
                Leaf = new Items("Leaf", 3);
                Leaf.Position = new Vector2(42.7f, 13.7f);
                Leaf.IsActive = true;
            }
            CameraMgr.CameraLimits = new CameraLimits(Game.Window.OrthoWidth * 2.5f, Game.Window.OrthoWidth * 0.5f, Game.Window.OrthoHeight * 2.5f, 1);

            //Keylist.Add(KeyForestPlace);
            #region MapRendering
            TmxReader reader = new TmxReader("Assets/Levels/Forest_Room.tmx");
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
                    //doorNumber += 1;
                    tileObj.RigidBody = new RigidBody(tileObj);
                    tileObj.RigidBody.Type = RigidBodyType.TileDoor;
                    tileObj.RigidBody.Collider = ColliderFactory.CreateBoxFor(tileObj, 3);
                    tileObj.RigidBody.AddCollisionType(RigidBodyType.Player);
                    //tileObj.RigidBody.AddCollisionType(RigidBodyType.Enemy);
                    //if (doorNumber <= 1)
                    //{
                    //    WithDoor.Add(tileObj, Keylist[0]);
                    //}
                    //else if (doorNumber <= 3 && doorNumber >= 2)
                    //{
                    //    WithDoor.Add(tileObj, Keylist[1]);
                    //}
                    //else if (doorNumber <= 5 && doorNumber >= 4)
                    //{
                    //    WithDoor.Add(tileObj, Keylist[2]);
                    //}
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

            player = new Player(Game.GetController(0), pathfinder, 5);
            player.Position = new Vector2(43, 23);
            player.IsActive = true;
            CameraMgr.SetTarget(player);

            Enemy = new Enemy(pathfinderMama, 5f);
        }
        protected override void LoadAssets()
        {
            GfxMgr.AddTexture("Pokeball", "Assets/Items/EasterEgg/Pokeball.png");
            GfxMgr.AddTexture("Mushroom", "Assets/Items/Mushroom.png");
            GfxMgr.AddTexture("Leaf", "Assets/Items/Leaf.png");
            base.LoadAssets();
        }
        public override void Draw()
        {
            AngelFeather.Draw();
            base.Draw();
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
