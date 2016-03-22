using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using monotest.Util;
using Nez;

namespace monotest.Components.World
{
    public class ChunkPhysics
        : Component
    {
        public override void onAddedToEntity()
        {
            var data = entity.getComponent<ChunkDataComponent>();

            for (var i = 0; i < 64; i++)
            {
                for (var j = 0; j < 64; j++)
                {
                    if (!TerrainGen.IsWalkable(data.BaseTileData[i, j])
                     || !TerrainGen.IsWalkable(data.DecorationTileData[i, j]))
                    {
                        entity.colliders.add(new BoxCollider(((data.ChunkX * 64) + i) * 16,
                            ((data.ChunkY * 64) + j) * 16, 16, 16));
                    }
                }
            }
        }
    }
}
