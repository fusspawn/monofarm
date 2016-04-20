using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using monotest.Components.Mob;
using monotest.Components.World;
using Nez;
using Nez.Console;

namespace Monotest.Components.Util { 

public class SelectedTile
        : Component, IUpdatable
    {

        public void update()
        {
            TileData Tdata = ChunkManager.Instance.MouseTile;
            if(Tdata == null)
                return;

            entity.transform.position = new Vector2(Tdata.TileX * ChunkManager.Instance.TileXPixels, 
                Tdata.TileY * ChunkManager.Instance.TileYPixels);


            if (Input.isKeyPressed(Keys.M) 
                && !DebugConsole.instance.isOpen)
            {
                TileData d = ChunkManager.Instance.MouseTile;
                if (d.IsTileWalkable)
                {
                    var ent = PlayerMaker.CreateDefaultMobEntity("mob", entity.scene);
                    ent.transform.position = ChunkManager.Instance.TileToWorld(d.TileX, d.TileY).ToVector2();
                }
            }
        }
    }
}
