using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using monotest.Rendering;
using monotest.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using IDrawable = monotest.Interfaces.IDrawable;
using monotest.Interfaces;
using Microsoft.Xna.Framework.Input;
using IUpdateable = monotest.Interfaces.IUpdateable;

namespace monotest.Objects
{
    public class TileMap
        : IDrawable, IUpdateable
    {
        public SpriteSheet TileSheet;
        private int[,] MapData;
        private int[,] BaseTileData;
        public Vector2 Offset = Vector2.Zero;
        public float Scale = 0f;
        public bool DistanceCull = true;
        public int RenderDistance = 20*64; // Render 20 tiles.

        public TileMap()
        {
            MapData = Noise2d.GenerateNoiseMap(500, 500, 8, 256);
            BaseTileData = new int[500, 500];
            for (var x = 0; x < 500; x++)
                for (var y = 0; y < 500; y++)
                    BaseTileData[x, y] = TerrainGen.TileForHeight(x, y, MapData[x, y]);
        }

        public int XTiles
        {
            get { return MapData.GetLength(0); }
        }

        public int YTiles
        {
            get { return MapData.GetLength(1); }
        }

        public Texture2D DebugTex;

        public void Draw(SpriteBatch B, GameTime T)
        {
            B.Begin();
            for (var x = 0; x < BaseTileData.GetLength(0); x++)
            {
                for (var y = 0; y < BaseTileData.GetLength(1); y++)
                {
                    B.Draw(TileSheet.Tex,
                        GetWorldPos(x, y) + Offset,
                        TileSheet.GetTileRect(BaseTileData[x, y]),
                        Color.White);
                }
            }

            if (DebugManager.Display)
            {
                Vector2 MouseTile = GetTileUnderMouse();
                B.Draw(DebugTex, GetWorldPos((int) MouseTile.X, (int) MouseTile.Y) + Offset, Color.White);
            }
            B.End();
        }

        public void LoadContent(ContentManager Manager)
        {
            TileSheet = new SpriteSheet();
            TileSheet.TileWidth = 64;
            TileSheet.TileHeight = 64;
            TileSheet.TileMargin = 10;
            TileSheet.Tex = Manager.Load<Texture2D>("spritesheet_tiles");
            DebugTex = Manager.Load<Texture2D>("select");
        }

        public Vector2 GetWorldPos(int XTile, int YTile)
        {
            return new Vector2(XTile*TileSheet.TileWidth,
                YTile*TileSheet.TileHeight);
        }

        public void Update(GameTime T)
        {
            Vector2 TilePos = GetTileUnderMouse();
            DebugManager.Values["MouseTileX"] = (TilePos.X).ToString();
            DebugManager.Values["MouseTileY"] = (TilePos.Y).ToString();
        }

        public Vector2 GetTileUnderMouse()
        {
            var MouseScreenPos = Mouse.GetState();
            Vector2 WorldPixPos = new Vector2(MouseScreenPos.X, MouseScreenPos.Y) - Offset;
            Vector2 TilePos = WorldPixPos/64;
            TilePos.X = (int) TilePos.X;
            TilePos.Y = (int) TilePos.Y;
            return TilePos;
        }

        public Vector2 GetTileAtScreenPos(Vector2 ScreenPos)
        {
            Vector2 WorldPixPos = new Vector2(ScreenPos.X, ScreenPos.Y) - Offset;
            Vector2 TilePos = WorldPixPos / 64;
            TilePos.X = (int)TilePos.X;
            TilePos.Y = (int)TilePos.Y;
            return TilePos;
        }
    }
}
