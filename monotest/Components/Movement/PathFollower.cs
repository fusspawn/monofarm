using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using monotest.Components.Util;
using monotest.Components.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Console;

namespace monotest.Components.Movement
{
    class PathFollower : Component, IUpdatable
    {
        Queue<PathNode> CurrentPath = new Queue<PathNode>();
        public override void onAddedToEntity()
        {
            Core.schedule(.5f, true, (onTime) =>
            {
                if (CurrentPath.Count > 0)
                {
                    var PathNode = CurrentPath.Dequeue();
                    entity.transform.position = new Vector2(PathNode.Tx * 16, PathNode.Ty * 16);
                }
            });
            base.onAddedToEntity();
        }

        public void update()
        {
            if (Input.isKeyPressed(Keys.N))
            {
                CurrentPath.Clear();
                Vector2 PlayerPos = MainGame.MainPlayerEnt.transform.position;
                PlayerPos = PlayerPos/16;

                DebugConsole.instance.log("Attempting Navigation");
                TileData MouseTile = ChunkManager.MouseTile;

                var Q = NavigationSystem.FindPath((int) PlayerPos.X, (int) PlayerPos.Y,
                    MouseTile.TileX, MouseTile.TileY);
                CurrentPath = new Queue<PathNode>(Q.Reverse());

                DebugConsole.instance.log("Found path of length: " + CurrentPath.Count);
            }

            
        }
    }
}
