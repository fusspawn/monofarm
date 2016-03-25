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
    }
}
