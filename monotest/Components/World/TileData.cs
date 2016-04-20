using monotest.Util;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monotest.Components.World
{
    public class TileData
    {
        private ChunkDataComponent Chunk;

        public int TileX;
        public int TileY;

        public int ChunkTileOffsetX;
        public int ChunkTileOffsetY;

        public int TileBaseType;
        public int TileDetailType;
        public bool IsTileWalkable;

        public Entity TileEntity;

        public void UpdateWalkableState()
        {
            if (TerrainGen.IsWalkable(TileBaseType) && TerrainGen.IsWalkable(TileDetailType) && TileEntity.getComponent<TIleCollider>() == null)
                IsTileWalkable = true;
            else
                IsTileWalkable = false;
        }
    }
}
