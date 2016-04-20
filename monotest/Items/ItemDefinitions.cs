using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monotest.Items
{
    public class ItemDefinitions
    {
        public Dictionary<string, Item> ItemDefs = new Dictionary<string, Item>();
        static ItemDefinitions _instance;

        public static ItemDefinitions Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ItemDefinitions();
                return _instance;
            }
        }

        public void CreateItemType(string Name, string Desc, int MaxStackSize)
        {
            Item I = new Item();
            I.Name = Name;
            I.Description = Desc;
            I.MaxStackSize = MaxStackSize;
            ItemDefs[I.Name] = I;
        }

        public Item GetItemType(string Name)
        {
            return ItemDefs[Name];
        }


        public void InteractWithItem(Item I, Entity Owner, Entity Target)
        {
            I.Interact(Owner, Target);
        }


        public void InteractWithItem(string ItemType, Entity Owner, Entity Target)
        {
            ItemDefs[ItemType]?.Interact(Owner, Target);
        }
    }
}