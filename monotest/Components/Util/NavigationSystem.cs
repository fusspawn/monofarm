using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using monotest.Components.World;
using Microsoft.Xna.Framework;
using Nez;
using Nez.Console;

namespace monotest.Components.Util
{
    public class NavigationSystem
    {
        public static Queue<PathNode> FindPath(int FromX, int FromY, int ToX, int ToY)
        {
            try
            {
                PathNode Start = new PathNode();
                Start.Tx = FromX;
                Start.Ty = FromY;

                PathNode End = new PathNode();
                End.Tx = ToX;
                End.Ty = ToY;

                Queue<PathNode> Path = new Queue<PathNode>();
                Dictionary<PathNode, PathNode> CameFrom = new Dictionary<PathNode, PathNode>();


                Queue<PathNode> Frontier = new Queue<PathNode>();
                Frontier.Enqueue(Start);

                PathNode Current;

                while (Frontier.Count > 0)
                {
                    Current = Frontier.Dequeue();

                    if (Current.Equals(End))
                    {
                        break;
                    }

                    foreach (PathNode nabour in GetNextDoorPathNodes(Current, End))
                    {
                        if (!CameFrom.ContainsKey(nabour))
                        {
                            Frontier.Enqueue(nabour);
                            CameFrom[nabour] = Current;
                        }
                    }
                }

                Current = End;
                Path.Enqueue(Current);

                while (!Current.Equals(Start))
                {
                    Current = CameFrom[Current];
                    Path.Enqueue(Current);
                }

                Path.Reverse();
                DebugPath(Path);

                return Path;
            }
            catch (Exception E)
            {
                DebugConsole.instance.log("Pathfinder Error: " + E.Message);
            }

            return new Queue<PathNode>();
        }

        public static void DebugPath(Queue<PathNode> Node)
        {
            PathNode[] Nodes = Node.ToArray();
            foreach (var n in Nodes)
            {
                Debug.drawHollowBox(new Vector2((n.Tx * ChunkManager.Instance.TileXPixels) +(int)(ChunkManager.Instance.TileXPixels /2), 
                    (n.Ty * ChunkManager.Instance.TileYPixels) + (int)(ChunkManager.Instance.TileYPixels /2)), ChunkManager.Instance.TileXPixels, Color.Green, 10);
            }
        }

        public static List<PathNode> GetNextDoorPathNodes(PathNode Location, PathNode End)
        {
            List<PathNode> RetList = new List<PathNode>();
            TileData Above = ChunkManager.Instance.GetTileData(Location.Tx, Location.Ty + 1);
            TileData Below = ChunkManager.Instance.GetTileData(Location.Tx, Location.Ty - 1);
            TileData Left = ChunkManager.Instance.GetTileData(Location.Tx - 1, Location.Ty);
            TileData Right = ChunkManager.Instance.GetTileData(Location.Tx + 1, Location.Ty);

            if (Above?.IsTileWalkable == true || ((Above.TileX == End.Tx) && (Above.TileY == End.Ty)))
            {
                PathNode Node = new PathNode();
                Node.Tx = (int)Above?.TileX;
                Node.Ty = (int)Above?.TileY;
                RetList.Add(Node);
            }

            if (Below?.IsTileWalkable == true || ((Below?.TileX == End.Tx) && (Below?.TileY == End.Ty)))
            {
                PathNode Node = new PathNode();
                Node.Tx = (int)Below?.TileX;
                Node.Ty = (int)Below?.TileY;
                RetList.Add(Node);
            }

            if (Left?.IsTileWalkable == true || ((Left?.TileX == End.Tx) && (Left?.TileY == End.Ty)))
            {
                PathNode Node = new PathNode();
                Node.Tx = (int)Left?.TileX;
                Node.Ty = (int)Left?.TileY;
                RetList.Add(Node);
            }

            if (Right?.IsTileWalkable == true || ((Right?.TileX == End.Tx) && (Right?.TileY == End.Ty)))
            {
                PathNode Node = new PathNode();
                Node.Tx = (int)Right?.TileX;
                Node.Ty = (int)Right?.TileY;
                RetList.Add(Node);
            }

            return RetList;

        }
    }

    public class PathNode
    {
        public int Tx;
        public int Ty;

        public override bool Equals(object obj)
        {
            PathNode other = (PathNode)obj;
            if (other.Tx == Tx && other.Ty == Ty)
                return true;

            return false;
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                // Suitable nullity checks etc, of course :)
                hash = hash * 23 + Tx.GetHashCode();
                hash = hash * 23 + Ty.GetHashCode();
                return hash;
            }
        }
    }
}
