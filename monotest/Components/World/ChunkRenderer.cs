﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using monotest.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Systems;

namespace monotest.Components.World
{
    class ChunkRenderer
        : RenderableComponent
    {
        private ChunkDataComponent Data;
        private Color DebugColor;
        private RenderTarget2D RenderTexture;
        private bool TextureDirty = false;


        public override void onAddedToEntity()
        {
            Data = entity.getComponent<ChunkDataComponent>() as ChunkDataComponent;           
            base.onAddedToEntity();
        }

        public override void render(Graphics graphics, Camera camera)
        {
            if (TextureDirty || RenderTexture == null)
                DrawChunkToTexture(graphics.spriteBatch);

            graphics.spriteBatch.Draw(RenderTexture, entity.transform.localPosition, Color.White);
        }

        public void DrawChunkToTexture(SpriteBatch B)
        {

            if (RenderTexture == null)
            {
                RenderTexture = new RenderTarget2D(B.GraphicsDevice,
                    Data.BaseTileData.GetLength(0) * ChunkManager.TileSheet.TileWidth,
                    Data.BaseTileData.GetLength(1) * ChunkManager.TileSheet.TileHeight);
            }


            B.End();
            RenderTarget2D OldTarget = B.GraphicsDevice.GetRenderTargets()[0].RenderTarget as RenderTarget2D;
            B.GraphicsDevice.SetRenderTarget(RenderTexture);


            B.Begin();
            Vector2 CachePos;
            for (var x = 0; x < Data.BaseTileData.GetLength(0); x++)
            {
                for (var y = 0; y < Data.BaseTileData.GetLength(1); y++)
                {
                    CachePos = new Vector2(x * ChunkManager.TileSheet.TileWidth,
                        y * ChunkManager.TileSheet.TileHeight);
                    B.Draw(ChunkManager.TileSheet.Tex,
                        CachePos, ChunkManager.TileSheet.GetTileRect(Data.BaseTileData[x, y]),
                        MainGame.DebugMode == true ? DebugColor : Color.White);
                }
            }
            B.End();
            B.Begin();
            for (var x = 0; x < Data.BaseTileData.GetLength(0); x++)
            {
                for (var y = 0; y < Data.BaseTileData.GetLength(1); y++)
                {
                    CachePos = new Vector2(x * ChunkManager.TileSheet.TileWidth, y * ChunkManager.TileSheet.TileHeight);
                    if (Data.DecorationTileData[x, y] != -1)
                    {
                        B.Draw(ChunkManager.TileSheet.Tex,
                            CachePos, ChunkManager.TileSheet.GetTileRect(Data.DecorationTileData[x, y]),
                            MainGame.DebugMode == true ? DebugColor : Color.White);
                    }
                }
            }
            B.End();
            B.GraphicsDevice.SetRenderTarget(OldTarget);
            B.Begin();
        }
    }
}
