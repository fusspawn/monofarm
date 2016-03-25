using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using monotest.Components.World;
using Microsoft.Xna.Framework;
using Nez;

namespace monotest.Components.Util
{
    public class SelectedTile 
        : Component, IUpdatable
    {

        public void update()
        {
            Vector2 WorldPos = entity.scene.camera.screenToWorldPoint(Input.rawMousePosition);

            WorldPos.X = (int)(WorldPos.X/ChunkManager.TileXPixels);
            WorldPos.Y = (int)(WorldPos.Y/ChunkManager.TileYPixels);
            WorldPos = WorldPos*16;
            WorldPos.X = WorldPos.X + 8;
            WorldPos.Y = WorldPos.Y + 8;
            entity.transform.position = WorldPos;
        }
    }
}
