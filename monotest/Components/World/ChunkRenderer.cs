using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using monotest.Rendering;
using monotest.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Systems;

namespace monotest.Components.World
{
    class ChunkRenderer
        : RenderableComponent , Nez.IUpdatable  
    {
        private ChunkDataComponent Data;
        private Color DebugColor;
        private RenderTarget2D RenderTexture;
        private Texture2D DebugTex;
        


        public override float width
        {
            get
            {
                return ChunkManager.Instance.ChunkWidth * ChunkManager.Instance.TileSheet.TileWidth;
            }
        }

        public override float height
        {
            get
            {
                return ChunkManager.Instance.ChunkHeight * ChunkManager.Instance.TileSheet.TileHeight;
            }
        }

        public override void onAddedToEntity()
        {
            renderLayer = 0;
            Data = entity.getComponent<ChunkDataComponent>();
            DebugTex = entity.scene.contentManager.Load<Texture2D>("select16");
            DebugColor = Nez.Random.nextColor();

            base.onAddedToEntity();
        }
        

        public override void render(Graphics graphics, Camera camera)
        {

            if(!ChunkManager.Instance.DisableRenderer)
                graphics.batcher.draw(RenderTexture, entity.transform.position, ChunkManager.Instance.DEBUG_PHYSICS ? DebugColor: Color.White);

            if (ChunkManager.Instance.DEBUG_PHYSICS)
            {
                Vector2Int CachePos;
                for (var x = 0; x < Data.ChunkTileData.GetLength(0); x++)
                {
                    for (var y = 0; y < Data.ChunkTileData.GetLength(1); y++)
                    {
                        if (!TerrainGen.IsWalkable(Data.ChunkTileData[x, y].TileBaseType)
                            || !TerrainGen.IsWalkable(Data.ChunkTileData[x, y].TileDetailType))
                        {
                            CachePos = 
                                new Vector2Int((x*ChunkManager.Instance.TileSheet.TileWidth),
                                    (y*ChunkManager.Instance.TileSheet.TileHeight));
                            CachePos.X += (int)entity.transform.position.X;
                            CachePos.Y += (int)entity.transform.position.Y;

                            graphics.batcher.draw(DebugTex,
                                CachePos.ToVector2(), Color.Red);
                        }
                    }
                }
            }
            
        }

        public void DrawChunkToTexture(Graphics graphics, Camera camera)
        {
            var B = graphics.batcher;

            if (RenderTexture == null)
            {
                RenderTexture = new RenderTarget2D(B.graphicsDevice,
                    Data.ChunkTileData.GetLength(0) * ChunkManager.Instance.TileSheet.TileWidth,
                    Data.ChunkTileData.GetLength(1) * ChunkManager.Instance.TileSheet.TileHeight);
            }


           // B.end();
            RenderTarget2D OldTarget = B.graphicsDevice.GetRenderTargets()[0].RenderTarget as RenderTarget2D;
            B.graphicsDevice.SetRenderTarget(RenderTexture);


            B.begin();
            Vector2Int CachePos;
            for (var x = 0; x < Data.ChunkTileData.GetLength(0); x++)
            {
                for (var y = 0; y < Data.ChunkTileData.GetLength(1); y++)
                {
                    DrawTile(x, y, B);
                }
            }
            B.end();
            B.graphicsDevice.SetRenderTarget(OldTarget);
           
            
            Data.IsDirty = false;
        }

        public void update()
        {
            if (Data.IsDirty || RenderTexture == null)
                DrawChunkToTexture(Graphics.instance, entity.scene.camera);
        }

        public void DrawTile(int x, int y, Batcher B)
        {
            var CachePos = new Vector2Int(x * ChunkManager.Instance.TileSheet.TileWidth,
                       y * ChunkManager.Instance.TileSheet.TileHeight);


            TileData tData = Data.ChunkTileData[x, y];
            B.draw(ChunkManager.Instance.TileSheet.Tex,
                CachePos.ToVector2(), ChunkManager.Instance.TileSheet.GetTileRect(tData.TileBaseType), Color.White);

            if(tData.TileDetailType != -1)
            {
                B.draw(ChunkManager.Instance.TileSheet.Tex,
                       CachePos.ToVector2(), ChunkManager.Instance.TileSheet.GetTileRect(Data.ChunkTileData[x, y].TileDetailType), Color.White);
            }
            
                if(tData.TileEntity.getComponent<TileComponent>() != null)
                {
                    tData.TileEntity.getComponent<TileComponent>().Draw(x, y, B, Data);
                }
            
        }
    }
}
