using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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
        public static ChunkManager Instance;
        public Entity[,] Chunks;
        public int MaxXChunks = 500;
        public int MaxYChunks = 500;
        public int ChunkWidth = 128;
        public int ChunkHeight = 128;
        public int MaxChunkRange = 5;
        public int TileXPixels = 16;
        public int TileYPixels = 16;
        public bool UseRenderTextures = true;

        public bool DisableRenderer = false;

        public SpriteSheet TileSheet;

        public Entity CmEntity;
        public bool DEBUG_PHYSICS = false;

        public TileData MouseTile;


        public ChunkManager()
        {
            Instance = this;
            Chunks = new Entity[MaxXChunks,MaxYChunks];
        }

        public int GetChunkIndex(int X, int Y)
        {
            return Y * MaxXChunks + X;
        }

        [Command("debug-chunk-physics", " true/false")]
        public void DebugPhysics(bool on)
        {
            DEBUG_PHYSICS = on;
        }

        [Command("render-world", "")]
        public void SetWorldRenderDisabled(bool enabled)
        {
            DisableRenderer = enabled;
        }

        public bool IsChunkLoaded(int X, int Y)
        {
            return Chunks[X,Y] != null;
        }

        public void BuildChunk(int X, int Y)
        {
            Entity CEnt = new Entity("chunk-"+X+"-"+Y);
            CmEntity.scene.addEntity(CEnt);

            CEnt.transform.position = ChunkToWorld(X, Y).ToVector2();
            var cdata = CEnt.addComponent<ChunkDataComponent>() as ChunkDataComponent;
            cdata.Init(X,Y);
            
            CEnt.addComponent<ChunkRenderer>();


            Chunks[X,Y] = CEnt;
        }

        public void update()
        {
            MouseTile = TileUnderMouse();
            SpawnUnloadedChunks();
            //CullChunksUneeded();
        }

        private void SpawnUnloadedChunks()
        {
            Vector2Int CameraChunkPos = WorldToChunk((int)CmEntity.scene.camera.position.X,
                (int)CmEntity.scene.camera.position.Y);

            int StartX = (int)(CameraChunkPos.X - (MaxChunkRange / 2));
            int StartY = (int)(CameraChunkPos.Y - (MaxChunkRange / 2));
            int EndX = (int)(CameraChunkPos.X + (MaxChunkRange / 2));
            int EndY = (int)(CameraChunkPos.Y + (MaxChunkRange / 2));


            for (int i = StartX; i < EndX; i++)
                for (int j = StartY; j < EndY; j++)
                    if ( InChunkBounds(i, j) && !IsChunkLoaded(i, j) )
                        BuildChunk(i, j);
        }

        private bool InChunkBounds(int i, int i1)
        {
            return i >= 0 && i <= MaxXChunks && i1 >= 0 && i1 <= MaxYChunks;
        }

        private void CullChunksUneeded()
        {
            
             Vector2Int CameraChunkPos = WorldToChunk((int)CmEntity.scene.camera.position.X,
                (int)CmEntity.scene.camera.position.Y);


            int StartX = (int)(CameraChunkPos.X - (MaxChunkRange / 2));
            int StartY = (int)(CameraChunkPos.Y - (MaxChunkRange / 2));
            int EndX = (int)(CameraChunkPos.X + (MaxChunkRange / 2));
            int EndY = (int)(CameraChunkPos.Y + (MaxChunkRange / 2));


            Rectangle ChunkRect = new Rectangle(StartX, StartY, EndX - StartX, EndY - StartY);
            List<Entity> Unload =  new List<Entity>();

            foreach (var Ent in Chunks)
            {
                ChunkDataComponent Comp = Ent.getComponent<ChunkDataComponent>();
                if (Vector2.Distance(CameraChunkPos.ToVector2(), new Vector2(Comp.ChunkX, Comp.ChunkY)) > MaxChunkRange * 1.5)
                {
                    Unload.Add(Ent);
                }
            }

            Unload.ForEach((c) =>
            {
                DebugConsole.instance.log("Removing Chunk");
                var comp = c.getComponent<ChunkDataComponent>();
                c.destroy();
                Chunks[comp.ChunkX, comp.ChunkY] = null;
            }); 
            
        }

        public Vector2Int WorldToChunk(int X, int Y)
        {
            return new Vector2Int(
                X / (ChunkWidth * TileXPixels),
                Y / (ChunkHeight * TileYPixels)
            );
        }

        public Vector2Int ChunkToWorld(int X, int Y)
        {
            return new Vector2Int(
                X * (ChunkWidth * TileXPixels),
                Y * (ChunkHeight * TileYPixels)
            );
        }

        public Vector2Int TileToWorld(int X, int Y)
        {
            return new Vector2Int(
                    X * TileXPixels,
                    Y * TileYPixels
             );
        }

        public Vector2Int TileToChunk(int X, int Y)
        {
            return new Vector2Int(
                    X / ChunkWidth,
                    Y / ChunkHeight
             );
        }

        public Vector2Int WorldToTile(int X, int Y)
        {
            return new Vector2Int(
                    X / TileXPixels,
                    Y / TileYPixels
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

        public TileData GetTileData(int Tx, int Ty)
        {
            Vector2Int ChunkLoc = TileToChunk(Tx, Ty);
            int tCX = ChunkLoc.X;
            int tCY = ChunkLoc.Y;

            int tCOffX = Math.Abs((int) (Tx%ChunkWidth));
            int tCOffY = Math.Abs((int) (Ty%ChunkHeight));

            if (!IsChunkLoaded(tCX, tCY))
            {
                return null;
            }


            TileData Data = new TileData();
            ChunkDataComponent cData = Chunks[tCX, tCY]?.getComponent<ChunkDataComponent>();

            if (cData == null)
            {
                return null;
            }

            return cData.ChunkTileData[tCOffX, tCOffY];
        }

        public void UpdateBaseTile(int X, int Y, int newType)
        {
            Vector2Int ChunkLoc = TileToChunk(X,Y);
            int tCX = ChunkLoc.X;
            int tCY = ChunkLoc.Y;

            int tCOffX = Math.Abs((int)(X % ChunkWidth));
            int tCOffY = Math.Abs((int)(Y % ChunkHeight));

            if (!IsChunkLoaded(tCX, tCY))
            {
                return;
            }


            TileData Data = new TileData();
            ChunkDataComponent cData = Chunks[tCX, tCY]?.getComponent<ChunkDataComponent>();

            if (cData == null)
            {
                return;
            }

            cData.ChunkTileData[tCOffX, tCOffY].TileBaseType = newType;
            cData.ChunkTileData[tCOffX, tCOffY].UpdateWalkableState();

            cData.IsDirty = true;
        }

        public void UpdateDetailTile(int X, int Y, int newType)
        {
            Vector2Int ChunkLoc = TileToChunk(X, Y);
            int tCX = ChunkLoc.X;
            int tCY = ChunkLoc.Y;

            int tCOffX = Math.Abs((int)(X % ChunkWidth));
            int tCOffY = Math.Abs((int)(Y % ChunkHeight));

            if (!IsChunkLoaded(tCX, tCY))
            {
                return;
            }
            
            ChunkDataComponent cData = Chunks[tCX, tCY]?.getComponent<ChunkDataComponent>();

            if (cData == null)
            {
                return;
            }

            cData.ChunkTileData[tCOffX, tCOffY].TileDetailType = newType;
            cData.ChunkTileData[tCOffX, tCOffY].UpdateWalkableState();

            cData.IsDirty = true;
        }


        public TileData TileUnderMouse()
        {
            Vector2 WorldPos = entity.scene.camera.screenToWorldPoint(Input.rawMousePosition);
            Vector2Int TilePos = WorldToTile((int)WorldPos.X, (int)WorldPos.Y);
            return GetTileData(TilePos.X, TilePos.Y);
        }

        public bool IsDetailAt(int X, int Y, int type)
        {
            return GetTileData(X, Y)?.TileDetailType == type;
        }

        public TileData SearchClosestDetail(int xs, int ys, int DetailType, int maxDistance=250)
        {

            if (IsDetailAt(xs, ys, DetailType))
                return GetTileData(xs, ys);

            for (int d = 1; d < maxDistance; d++)
            {
                for (int i = 0; i < d + 1; i++)
                {
                    int x1 = xs - d + i;
                    int y1 = ys - i;

                    // Check point (x1, y1)
                    if (IsDetailAt(x1, y1, DetailType))
                        return GetTileData(x1, y1);

                    int x2 = xs + d - i;
                    int y2 = ys + i;

                    // Check point (x2, y2)
                    if (IsDetailAt(x2, y2, DetailType))
                        return GetTileData(x2, y2);
                }


                for (int i = 1; i < d; i++)
                {
                    int x1 = xs - i;
                    int y1 = ys + d - i;

                    // Check point (x1, y1)
                    if (IsDetailAt(x1, y1, DetailType))
                        return GetTileData(x1, y1);

                    int x2 = xs + d - i;
                    int y2 = ys - i;

                    // Check point (x2, y2)
                    if (IsDetailAt(x2, y2, DetailType))
                        return GetTileData(x2, y2);
                }
            }

            return null;
        }


        public bool IsResourceAt(int X, int Y, string Name)
        {
            var res = false;
            if( GetTileData(X, Y)?.TileEntity.getComponent<ResourceTile>()?.ResourceName == Name )
                    res = true;
            return res;
        }

        public TileData SearchClosestResource(int xs, int ys, string Name, int maxDistance = 250)
        {

            if (IsResourceAt(xs, ys, Name))
                return GetTileData(xs, ys);

            for (int d = 1; d < maxDistance; d++)
            {
                for (int i = 0; i < d + 1; i++)
                {
                    int x1 = xs - d + i;
                    int y1 = ys - i;

                    // Check point (x1, y1)
                    if (IsResourceAt(x1, y1, Name))
                        return GetTileData(x1, y1);

                    int x2 = xs + d - i;
                    int y2 = ys + i;

                    // Check point (x2, y2)
                    if  (IsResourceAt(x2, y2, Name))
                        return GetTileData(x2, y2);
                }


                for (int i = 1; i < d; i++)
                {
                    int x1 = xs - i;
                    int y1 = ys + d - i;

                    // Check point (x1, y1)
                    if (IsResourceAt(x1, y1, Name))
                        return GetTileData(x1, y1);

                    int x2 = xs + d - i;
                    int y2 = ys - i;

                    // Check point (x2, y2)
                    if (IsResourceAt(x2, y2, Name))
                        return GetTileData(x2, y2);
                }
            }

            return null;
        }


        public void HarvestResource(int x, int y, string Type, Entity target)
        {
            if (GetTileData(x, y)?.TileEntity.getComponent<ResourceTile>()?.ResourceName == Type)
                GetTileData(x, y)?.TileEntity.getComponent<ResourceTile>()?.Harvest(target);

            GetTileData(x, y)?.UpdateWalkableState();

            Vector2Int ChunkLoc = TileToChunk(x, y);
            int tCX = ChunkLoc.X;
            int tCY = ChunkLoc.Y;

            if (!IsChunkLoaded(tCX, tCY))
            {
                return;
            }

            ChunkDataComponent cData = Chunks[tCX, tCY]?.getComponent<ChunkDataComponent>();
            cData.IsDirty = true;
        }

    }
}
