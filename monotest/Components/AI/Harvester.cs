using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using monotest.Components.Movement;
using monotest.Components.World;
using monotest.Util;
using Nez;

namespace monotest.Components.AI
{
    public class Harvester : Component
    {
        private TileData Target;
        private PathFollower Pather;
        public int HarvestType;


        public override void onAddedToEntity()
        {
            Pather = entity.getComponent<PathFollower>();
            Core.schedule(1f, true, (onTime) =>
            {
                ai();
            });
           base.onAddedToEntity();
        }


        public void ai()
        {
            if (Pather.HasPath)
            {
                SanityCheckTarget();
                return;
            }

            if (Target == null)
            {
                Vector2Int CurrentPos = ChunkManager.Instance.WorldToTile((int) entity.transform.position.X,
                    (int) entity.transform.position.Y);
                Target = ChunkManager.Instance.SearchClosestDetail(CurrentPos.X, CurrentPos.Y, 527);

                if (Target != null)
                {
                    Pather.SetPath(Target.TileX, Target.TileY);
                }

                return;
            }


            ChunkManager.Instance.UpdateDetailTile(Target.TileX,Target.TileY, -1);
            Target = null;
        }

        private void SanityCheckTarget()
        {
            if (Target == null)
            {
                Pather.ClearPath();
            }

            TileData Dat = ChunkManager.Instance.GetTileData(Target.TileX, Target.TileY);
            if (Dat?.TileDetailType != Target?.TileDetailType)
            {

                //Target no longer there
                Pather.ClearPath();
                Target = null;
            }
        }
    }
}
