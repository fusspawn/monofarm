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

namespace monotest.Components.World
{
    class ChunkManager : Component
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

        public static SpriteSheet TileSheet;

        public static Entity CmEntity;

        public static int GetChunkIndex(int X, int Y)
        {
            return Y * MaxXChunks + X;
        }

        public static bool IsChunkLoaded(int X, int Y)
        {
            return ChunkCache.ContainsKey(GetChunkIndex(X, Y));
        }

        public void BuildChunk(int X, int Y)
        {
            Entity CEnt = new Entity("chunk-"+X+"-"+Y);
            CEnt.transform.localPosition = ChunkToWorld(X, Y);
            var cdata = CEnt.addComponent<ChunkDataComponent>() as ChunkDataComponent;
            cdata.Init(X,Y);
            CEnt.addComponent<ChunkRenderer>();
            CEnt.transform.parent = CmEntity.transform;
        }

        public void Update(GameTime T)
        {
            SpawnUnloadedChunks();
            CullChunksUneeded();

            if (Keyboard.GetState().IsKeyDown(Keys.F11))
                MainGame.DebugMode = !MainGame.DebugMode;
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
            Vector2 CameraChunkPos = WorldToChunk((int)MainGame.MainCamera.Pos.X,
                (int)MainGame.MainCamera.Pos.Y);

            int StartX = (int)(CameraChunkPos.X - (MaxChunkRange / 2));
            int StartY = (int)(CameraChunkPos.Y - (MaxChunkRange / 2));
            int EndX = (int)(CameraChunkPos.X + (MaxChunkRange / 2));
            int EndY = (int)(CameraChunkPos.Y + (MaxChunkRange / 2));


            Rectangle ChunkRect = new Rectangle(StartX, StartY, EndX - StartX, EndY - StartY);
            List<int> Unload = (from c in ChunkCache.AsParallel()
                                where !ChunkRect.Contains(c.Value.ChunkX, c.Value.ChunkY)
                                select c.Key).ToList();

            Unload.ForEach((c) =>
            {
                DebugConsole.instance.log("Removing Chunk");
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
    }
}
