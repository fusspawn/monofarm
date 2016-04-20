using monotest.Components.Mob;
using monotest.Items;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monotest.Components.World
{
    public class ResourceTile
        : TileComponent
    {
        public string ResourceName;
        public bool DestroyOnHarvest = false;
        private TileData internaldata;
        
        public ResourceTile(string _ResourceName, int TileTexID, TileData tdata, bool _DestroyOnHarvest = true)
        {
            ResourceName = _ResourceName;
            TexID = TileTexID;
            DestroyOnHarvest = _DestroyOnHarvest;
            internaldata = tdata;
            
        }

        public override void onAddedToEntity()
        {
            base.onAddedToEntity();
        }

        public void Harvest(Entity E)
        {
            Inventory I = E.getComponent<Inventory>();
            I.AddItem(ItemDefinitions.Instance.GetItemType(ResourceName), 1);

            if (DestroyOnHarvest)
            {
                internaldata.TileEntity.removeComponent(this);
                internaldata.TileEntity.removeComponent<TIleCollider>();
                internaldata.UpdateWalkableState();
            }
        }
    }
}
