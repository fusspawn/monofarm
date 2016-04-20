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
        

        public int PathNodeCount
        {
            get { return CurrentPath.Count; }
        }
        public bool HasPath {
            get { return CurrentPath.Count > 0; }
        }

        public PathNode NextNode => CurrentPath.Peek();

        public override void onAddedToEntity()
        {
            Core.schedule(1f, true, (onTime) =>
            {
                if (CurrentPath.Count > 0)
                {
                    var PathNode = CurrentPath.Dequeue();
                    entity.transform.position = ChunkManager.Instance.TileToWorld(PathNode.Tx, PathNode.Ty).ToVector2();
                }
            });

            base.onAddedToEntity();
        }

        public void update()
        {
            if (Input.isKeyPressed(Keys.N) 
                && !DebugConsole.instance.isOpen)
            {
                CurrentPath.Clear();
                Vector2 PlayerPos = entity.transform.position;

                PlayerPos = PlayerPos/16;

                DebugConsole.instance.log("Attempting Navigation");
                TileData MouseTile = ChunkManager.Instance.MouseTile;

                if (MouseTile == null)
                {
                    DebugConsole.instance.log("Cant find mousetile. Unable to build navpath to null dest!");
                    return;
                }

                Queue<PathNode> Q = NavigationSystem.FindPath((int) PlayerPos.X, (int) PlayerPos.Y,
                    MouseTile.TileX, MouseTile.TileY);
                CurrentPath = new Queue<PathNode>(Q.Reverse());

                DebugConsole.instance.log("Found path of length: " + CurrentPath.Count);
            }

            
        }

        public void SetPath(int X, int Y)
        {
            Vector2 PlayerPos = entity.transform.position;
            PlayerPos = PlayerPos / 16;

            Queue<PathNode> Q = NavigationSystem.FindPath((int)PlayerPos.X, (int)PlayerPos.Y,
                X, Y);
            CurrentPath = new Queue<PathNode>(Q.Reverse());
        }

        public void ClearPath()
        {
            CurrentPath.Clear();
        }
    }
}
