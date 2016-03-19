using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using monotest.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using IDrawable = monotest.Interfaces.IDrawable;
using IUpdateable = monotest.Interfaces.IUpdateable;

namespace monotest.Objects
{
    public class ChunkedWorld 
        : IUpdateable, IDrawable
    {
        public static Dictionary<int, Chunk> ChunkCache
            = new Dictionary<int, Chunk>();

        public static int MaxXChunks = 50;
        public static int MaxYChunks = 50;
        public static int ChunkWidth = 64;
        public static int ChunkHeight = 64;
        public static int MaxChunkRange = 8;
        public static int TileXPixels = 16;
        public static int TileYPixels = 16;

        public static void Init()
        {
            Noise2d.MaxX = MaxXChunks * ChunkWidth;
            Noise2d.MaxY = MaxYChunks * ChunkHeight;
        } 

        public static int GetChunkIndex(int X, int Y)
        {
            return Y*ChunkedWorld.MaxXChunks + X;
        }

        public static bool IsChunkLoaded(int X, int Y)
        {
            return ChunkCache.ContainsKey(GetChunkIndex(X, Y));
        }

        public static void BuildChunk(int X, int Y)
        {
            ChunkCache[GetChunkIndex(X, Y)] = new Chunk(X, Y);
            ChunkCache[GetChunkIndex(X, Y)].LoadContent(MainGame.ContentManager);
            Console.WriteLine("BuildChunk("+ X +"," + Y + ");");
        }

        public void Update(GameTime T)
        {
            SpawnUnloadedChunks();
            CullChunksUneeded();

            if (Keyboard.GetState().IsKeyDown(Keys.F11))
                MainGame.DebugMode = !MainGame.DebugMode;

            foreach (var keyValuePair in ChunkCache)
            {
                keyValuePair.Value.Update(T);
            }

            DebugManager.Values["Loaded Chunks"] = ChunkCache.Count.ToString();
            Vector2 ChunkPosForPlayer = WorldToChunk((int)MainGame.MainPlayer.Location.X, (int)MainGame.MainPlayer.Location.Y);
            DebugManager.Values["PlayerInChunk"] = "X: " + ChunkPosForPlayer.X + " Y: " + ChunkPosForPlayer.Y;
        }

        private void SpawnUnloadedChunks()
        {
            Vector2 CameraChunkPos = WorldToChunk((int)MainGame.MainCamera.Pos.X, 
                (int)MainGame.MainCamera.Pos.Y);

            int StartX = (int)(CameraChunkPos.X - (MaxChunkRange / 2));
            int StartY = (int)(CameraChunkPos.Y - (MaxChunkRange / 2));
            int EndX = (int)(CameraChunkPos.X + (MaxChunkRange / 2));
            int EndY = (int)(CameraChunkPos.Y + (MaxChunkRange / 2));


            for (int i = StartX; i < EndX; i++)
                for(int j = StartY; j < EndY; j++)
                    if(!IsChunkLoaded(i, j))
                        BuildChunk(i, j);
        }

        private void CullChunksUneeded()
        {
            Vector2 CameraChunkPos = WorldToChunk((int)MainGame.MainCamera.Pos.X,
                (int)MainGame.MainCamera.Pos.Y);

            int StartX = (int)(CameraChunkPos.X - (MaxChunkRange / 2));
            int StartY = (int)(CameraChunkPos.Y - (MaxChunkRange / 2));
            int EndX = (int)(CameraChunkPos.X + (MaxChunkRange / 2));
            int EndY = (int)(CameraChunkPos.Y + (MaxChunkRange / 2));


            Rectangle ChunkRect = new Rectangle(StartX, StartY, EndX-StartX, EndY-StartY);
            List<int> Unload = (from c in ChunkCache.AsParallel()
                                where !ChunkRect.Contains(c.Value.ChunkX, c.Value.ChunkY)
                                select c.Key).ToList();

            Unload.ForEach((c) =>
            {
                Console.WriteLine("Removing Chunk");
                ChunkCache.Remove(c);
            });
            
        }

        public void Draw(SpriteBatch B, GameTime T)
        {
            foreach (var keyValuePair in ChunkCache)
            {
                keyValuePair.Value.Draw(B, T);
            }
        }

        public void LoadContent(ContentManager Manager)
        {
            foreach (var keyValuePair in ChunkCache)
            {
                keyValuePair.Value.LoadContent(Manager);
            }
        }


        public Vector2 WorldToChunk(int X, int Y)
        {
            return new Vector2(
                (int)(X / (ChunkWidth * TileXPixels)),
                (int)(Y / (ChunkHeight * TileYPixels))
            );
        }

        public Vector2 TileToWorld(int X, int Y)
        {
            return new Vector2(
                X * TileXPixels,
                Y * TileYPixels
            );
        }


        public Vector2 WorldToTile(int X, int Y)
        {
            return new Vector2(
               (int)( X / TileXPixels),
               (int)( Y / TileYPixels)
            );
        }

        public Vector2 ChunkToTile(int X, int Y)
        {
            return new Vector2(
                X * ChunkWidth,
                Y * ChunkHeight
            );
        }
    }
}
