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

        public static int GetChunkIndex(int X, int Y)
        {
            return Y * MaxXChunks + X;
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

            CEnt.addComponent<ChunkPhysics>();
            CEnt.addComponent<ChunkRenderer>();


            ChunkCache[GetChunkIndex(X, Y)] = CEnt;
        }

        public void update()
        {
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
            /*
             Vector2 CameraChunkPos = WorldToChunk((int)CmEntity.scene.camera.position.X,
                (int)CmEntity.scene.camera.position.Y);


            int StartX = (int)(CameraChunkPos.X - (MaxChunkRange / 2));
            int StartY = (int)(CameraChunkPos.Y - (MaxChunkRange / 2));
            int EndX = (int)(CameraChunkPos.X + (MaxChunkRange / 2));
            int EndY = (int)(CameraChunkPos.Y + (MaxChunkRange / 2));


            Rectangle ChunkRect = new Rectangle(StartX, StartY, EndX - StartX, EndY - StartY);
            List<KeyValuePair<int, Entity>> Unload = (from c in ChunkCache.AsParallel()
                                where !ChunkRect.Contains(c.Value.ChunkX, c.Value.ChunkY)
                                select c).ToList();

            Unload.ForEach((c) =>
            {
                DebugConsole.instance.log("Removing Chunk");
                CmEntity.scene.entities.remove(c);
                ChunkCache.Remove(c);
            }); 
            */
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
    }
}
