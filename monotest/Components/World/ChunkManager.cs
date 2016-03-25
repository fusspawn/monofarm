using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using monotest.Rendering;
using Nez;
using Microsoft.Xna.Framework;
using Nez.Console;
using monotest.Util;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace monotest.Components.World
{
    class ChunkManager : Component, IUpdatable
    {
        public static Dictionary<int, Entity> ChunkCache
            = new Dictionary<int, Entity>();

        public static int MaxXChunks = 50;
        public static int MaxYChunks = 50;
        public static int ChunkWidth = 64;
        public static int ChunkHeight = 64;
        public static int MaxChunkRange = 8;
        public static int TileXPixels = 16;
        public static int TileYPixels = 16;
        public static bool UseRenderTextures = true;

        public static bool DisableRenderer = false;

        public static SpriteSheet TileSheet;

        public static Entity CmEntity;
        public static bool DEBUG_PHYSICS = false;

        public static TileData MouseTile;


        public static int GetChunkIndex(int X, int Y)
        {
            return Y * MaxXChunks + X;
        }


        [Command("debug-chunk-phys", " true/false ")]
        public static void DebugPhysics(bool on)
        {
            DEBUG_PHYSICS = on;
        }

        [Command("render-world", "")]
        public static void SetWorldRenderDisabled(bool enabled)
        {
            DisableRenderer = enabled;
        }

        public static bool IsChunkLoaded(int X, int Y)
        {
            return ChunkCache.ContainsKey(GetChunkIndex(X, Y));
        }

        public void BuildChunk(int X, int Y)
        {
            Entity CEnt = new Entity("chunk-"+X+"-"+Y);
            CmEntity.scene.addEntity(CEnt);

            CEnt.transform.position = ChunkToWorld(X, Y);

            var cdata = CEnt.addComponent<ChunkDataComponent>() as ChunkDataComponent;
            cdata.Init(X,Y);

            //CEnt.addComponent<ChunkPhysics>();
            CEnt.addComponent<ChunkRenderer>();


            ChunkCache[GetChunkIndex(X, Y)] = CEnt;
        }

        public void update()
        {
            MouseTile = TileUnderMouse();
            SpawnUnloadedChunks();
            CullChunksUneeded();
        }

        private void SpawnUnloadedChunks()
        {
            Vector2 CameraChunkPos = WorldToChunk((int)CmEntity.scene.camera.position.X,
                (int)CmEntity.scene.camera.position.Y);

            int StartX = (int)(CameraChunkPos.X - (MaxChunkRange / 2));
            int StartY = (int)(CameraChunkPos.Y - (MaxChunkRange / 2));
            int EndX = (int)(CameraChunkPos.X + (MaxChunkRange / 2));
            int EndY = (int)(CameraChunkPos.Y + (MaxChunkRange / 2));


            for (int i = StartX; i < EndX; i++)
                for (int j = StartY; j < EndY; j++)
                    if (!IsChunkLoaded(i, j))
                        BuildChunk(i, j);
        }

        private void CullChunksUneeded()
        {
            
             Vector2 CameraChunkPos = WorldToChunk((int)CmEntity.scene.camera.position.X,
                (int)CmEntity.scene.camera.position.Y);


            int StartX = (int)(CameraChunkPos.X - (MaxChunkRange / 2));
            int StartY = (int)(CameraChunkPos.Y - (MaxChunkRange / 2));
            int EndX = (int)(CameraChunkPos.X + (MaxChunkRange / 2));
            int EndY = (int)(CameraChunkPos.Y + (MaxChunkRange / 2));


            Rectangle ChunkRect = new Rectangle(StartX, StartY, EndX - StartX, EndY - StartY);
            List<KeyValuePair<int, Entity>> Unload =  new List<KeyValuePair<int, Entity>>();

            foreach (var Ent in ChunkCache)
            {
                ChunkDataComponent Comp = Ent.Value.getComponent<ChunkDataComponent>();
                if (Vector2.Distance(CameraChunkPos, new Vector2(Comp.ChunkX, Comp.ChunkY)) > MaxChunkRange * 1.5)
                {
                    Unload.Add(Ent);
                }
            }

            Unload.ForEach((c) =>
            {
                DebugConsole.instance.log("Removing Chunk");
                c.Value.destroy();
                ChunkCache.Remove(c.Key);
            }); 
            
        }

        public Vector2 WorldToChunk(int X, int Y)
        {
            return new Vector2(
                (int)(X / (ChunkWidth * TileXPixels)),
                (int)(Y / (ChunkHeight * TileYPixels))
            );
        }

        public Vector2 ChunkToWorld(int X, int Y)
        {
            return new Vector2(
                (int)(X * (ChunkWidth * TileXPixels)),
                (int)(Y * (ChunkHeight * TileYPixels))
            );
        }

        public override void onAddedToEntity()
        {
            TileSheet = new SpriteSheet();
            TileSheet.TileWidth = 16;
            TileSheet.TileHeight = 16;
            TileSheet.TileMargin = 1;
            TileSheet.Tex = entity.scene.contentManager.Load<Texture2D>("roguelikeSheet_transparent");
        }

        public static TileData GetTileData(int Tx, int Ty)
        {
            int tCX = (int) (Tx/ChunkWidth);
            int tCY = (int) (Ty/ChunkHeight);
            int tCOffX = Math.Abs((int) (Tx%ChunkWidth));
            int tCOffY = Math.Abs((int) (Ty%ChunkHeight));

            if (!IsChunkLoaded(tCX, tCY))
            {
                return null;
            }


            TileData Data = new TileData();
            ChunkDataComponent cData = ChunkCache[GetChunkIndex(tCX, tCY)].getComponent<ChunkDataComponent>();

            if (cData == null)
            {
                return null;
            }

            Data.ChunkTileOffsetX = tCOffX;
            Data.ChunkTileOffsetY = tCOffY;
            Data.TileX = Tx;
            Data.TileY = Ty;
            Data.TileBaseType = cData.BaseTileData[tCOffX, tCOffY];
            Data.TileDetailType = cData.DecorationTileData[tCOffX, tCOffY];
            Data.IsTileWalkable = (TerrainGen.IsWalkable(Data.TileBaseType) &&
                                   TerrainGen.IsWalkable(Data.TileDetailType));

            return Data;
        }


        public TileData TileUnderMouse()
        {
            Vector2 WorldPos = entity.scene.camera.screenToWorldPoint(Input.rawMousePosition);

            WorldPos.X = (int)(WorldPos.X / ChunkManager.TileXPixels);
            WorldPos.Y = (int)(WorldPos.Y / ChunkManager.TileYPixels);

            return GetTileData((int)WorldPos.X, (int)WorldPos.Y);
        }
    }
}
