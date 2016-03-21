using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using monotest.Rendering;
using monotest.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace monotest.Objects
{
    public class Chunk
    {
        public int ChunkX;
        public int ChunkY;
        public SpriteSheet TileSheet;
        private int[,] MapData;
        private int[,] BaseTileData;
        private int[,] DecorationTileData;
        private List<ChunkStaticObject> ChunkObjects = new List<ChunkStaticObject>(); 
         
        private Color DebugColor;
        private SpriteFont DebugFont;
        private ContentManager ContentInstance;

        private RenderTarget2D RenderTexture;
        private bool TextureDirty = false;


        public bool IsGenerated = false;
        public bool IsGenerating = false;
        private Task GenerateTask;

        public Chunk(int x, int y)
        {
            this.ChunkX = x;
            this.ChunkY = y;

            IsGenerating = true;

            Random R = new Random();
            DebugColor = new Color(R.Next(255), 
                R.Next(255), R.Next(255));


            //GenerateTask = Task.Run(() =>
            //{

                MapData = Noise2d.GenerateNoiseMap(ChunkX*64, ChunkY*64,
                    64, 64, 8, 256);
                BaseTileData = new int[64, 64];
                DecorationTileData = new int[64, 64];

                for (var i = 0; i < 64; i++)
                {
                    for (var j = 0; j < 64; j++)
                    {
                        BaseTileData[i, j]
                            = TerrainGen.TileForHeight((ChunkX*64) + i, (ChunkY*64) + j,
                                MapData[i, j]);

                        DecorationTileData[i, j] = TerrainGen.DecorationForTile(
                            (ChunkX*64) + i, (ChunkY*64) + j, MapData[i, j]);
                    }
                }

                IsGenerating = false;
                IsGenerated = true;
            //});
        }

        public void Update(GameTime gameTime)
        {
            if (!IsGenerated)
                return;

            foreach (ChunkStaticObject Obj in ChunkObjects)
            {
                Obj.Update(gameTime);
            }
        }

        public void LoadContent(ContentManager Manager)
        {
            ContentInstance = Manager;
            TileSheet = new SpriteSheet();
            TileSheet.TileWidth = 16;
            TileSheet.TileHeight = 16;
            TileSheet.TileMargin = 1;
            TileSheet.Tex = Manager.Load<Texture2D>("roguelikeSheet_transparent");
            DebugFont = Manager.Load<SpriteFont>("debugfont");
        }

        public void Draw(SpriteBatch B, GameTime gameTime)
        {

            if (!IsGenerated)
                return;

            if (ChunkedWorld.UseRenderTextures)
            {
                if (RenderTexture == null || TextureDirty)
                {
                    DrawChunkToTexture(B, gameTime);
                }

                Vector2 StartPos = GetWorldPos((ChunkX*ChunkedWorld.ChunkHeight),
                    (ChunkY*ChunkedWorld.ChunkHeight));
                B.Begin(SpriteSortMode.BackToFront, null, null, null,
                    null, null, MainGame.MainCamera.GetTransformation(B.GraphicsDevice));
                B.Draw(RenderTexture, StartPos, MainGame.DebugMode == true ? DebugColor : Color.White);
                B.End();
            }
            else { 

                B.Begin(SpriteSortMode.BackToFront, null, null, null,
                    null, null, MainGame.MainCamera.GetTransformation(B.GraphicsDevice));
                Vector2 CachePos;
                for (var x = 0; x < BaseTileData.GetLength(0); x++)
                {
                    for (var y = 0; y < BaseTileData.GetLength(1); y++)
                    {
                        CachePos = GetWorldPos(x + (ChunkX*ChunkedWorld.ChunkHeight),
                            y + (ChunkY*ChunkedWorld.ChunkHeight));

                        B.Draw(TileSheet.Tex,
                            CachePos, TileSheet.GetTileRect(BaseTileData[x, y]),
                            MainGame.DebugMode == true ? DebugColor : Color.White);
                    }
                }
                B.End();
                B.Begin(SpriteSortMode.BackToFront, null, null, null,
                    null, null, MainGame.MainCamera.GetTransformation(B.GraphicsDevice));
                for (var x = 0; x < BaseTileData.GetLength(0); x++)
                {
                    for (var y = 0; y < BaseTileData.GetLength(1); y++)
                    {
                        CachePos = GetWorldPos(x + (ChunkX*ChunkedWorld.ChunkHeight),
                            y + (ChunkY*ChunkedWorld.ChunkHeight));

                        if (DecorationTileData[x, y] != -1)
                        {
                            B.Draw(TileSheet.Tex,
                                CachePos, TileSheet.GetTileRect(DecorationTileData[x, y]),
                                MainGame.DebugMode == true ? DebugColor : Color.White);
                        }
                    }
                }
                B.End();
            }
        }

        public void DrawChunkToTexture(SpriteBatch B, GameTime gameTime)
        {

            if (RenderTexture == null)
            {
                RenderTexture = new RenderTarget2D(B.GraphicsDevice, 
                    BaseTileData.GetLength(0) * TileSheet.TileWidth, 
                    BaseTileData.GetLength(1) * TileSheet.TileHeight);
            }

            B.GraphicsDevice.SetRenderTarget(RenderTexture);
            B.Begin();
            Vector2 CachePos;
            for (var x = 0; x < BaseTileData.GetLength(0); x++)
            {
                for (var y = 0; y < BaseTileData.GetLength(1); y++)
                {
                    CachePos = new Vector2(x * TileSheet.TileWidth, y * TileSheet.TileHeight);

                    B.Draw(TileSheet.Tex,
                        CachePos, TileSheet.GetTileRect(BaseTileData[x, y]),
                        MainGame.DebugMode == true ? DebugColor : Color.White);
                }
            }
            B.End();
            B.Begin();
            for (var x = 0; x < BaseTileData.GetLength(0); x++)
            {
                for (var y = 0; y < BaseTileData.GetLength(1); y++)
                {
                    CachePos = new Vector2(x * TileSheet.TileWidth, y * TileSheet.TileHeight);

                    if (DecorationTileData[x, y] != -1)
                    {
                        B.Draw(TileSheet.Tex,
                            CachePos, TileSheet.GetTileRect(DecorationTileData[x, y]),
                            MainGame.DebugMode == true ? DebugColor : Color.White);
                    }
                }
            }
            B.End();
            B.GraphicsDevice.SetRenderTarget(null);
        }

        public Vector2 GetWorldPos(int XTile, int YTile)
        {
            return new Vector2(XTile * TileSheet.TileWidth,
                YTile * TileSheet.TileHeight);
        }
    }
}
