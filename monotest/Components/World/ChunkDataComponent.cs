using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using monotest.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using monotest.Util;

namespace monotest.Components.World
{
    class ChunkDataComponent : Component
    {

        public int ChunkX;
        public int ChunkY;

        public int[,] MapData;
        public int[,] BaseTileData;
        public int[,] DecorationTileData;
        
        public void Init(int X, int Y)
        {
            ChunkX = X;
            ChunkY = Y;

            MapData = Noise2d.GenerateNoiseMap(ChunkX * 64, ChunkY * 64,
             64, 64, 8, 256);
            BaseTileData = new int[64, 64];
            DecorationTileData = new int[64, 64];

            for (var i = 0; i < 64; i++)
            {
                for (var j = 0; j < 64; j++)
                {
                    BaseTileData[i, j]
                        = TerrainGen.TileForHeight((ChunkX * 64) + i, (ChunkY * 64) + j,
                            MapData[i, j]);

                    DecorationTileData[i, j] = TerrainGen.DecorationForTile(
                        (ChunkX * 64) + i, (ChunkY * 64) + j, MapData[i, j]);

                    if (!TerrainGen.IsWalkable(BaseTileData[i, j])
                        || !TerrainGen.IsWalkable(DecorationTileData[i, j]))
                    {
                        entity.colliders.add(new BoxCollider((i * ChunkManager.TileSheet.TileWidth),
                            (j * ChunkManager.TileSheet.TileHeight), ChunkManager.TileSheet.TileWidth,
                            ChunkManager.TileSheet.TileHeight));
                    }
                }
            }
        }


        
    }
}
