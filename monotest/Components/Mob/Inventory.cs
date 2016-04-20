using monotest.Items;
using Nez;
using Nez.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monotest.Components.Mob
{
    public class Inventory 
        : Component
    {
        public int MaxItemCount = 10;
        public List<ItemStack> InventoryList;

        public Inventory(int _MaxItemCount)
        {
            MaxItemCount = _MaxItemCount;
            InventoryList = new List<ItemStack>(MaxItemCount);
        }

        public void AddItem(Item item, int Amount)
        {
            var existing = GetExistingStack(item);
            if(existing != null)
            {
                existing.StackSize += Amount;
            }
            else
            {
                if (InventoryList.Count < MaxItemCount)
                    InventoryList.Add(new ItemStack(item.Name, Amount));
            }
        }

        public ItemStack GetExistingStack(Item I)
        {
            foreach(ItemStack IStack in InventoryList)
            {
                if (IStack.Name == I.Name)
                    return IStack;
            }

            return null;
        }

        public ItemStack GetExistingStack(string ItemName)
        {
            foreach (ItemStack IStack in InventoryList)
            {
                if (IStack.Name == ItemName)
                    return IStack;
            }

            return null;
        }


        public int GetItemCount(string ItemName)
        {
            foreach (ItemStack IStack in InventoryList)
                if (IStack.Name == ItemName)
                    return IStack.StackSize;

            return 0;
        }

        public int GetItemCount(Item Item)
        {
            foreach (ItemStack IStack in InventoryList)
                if (IStack.Name == Item.Name)
                    return IStack.StackSize;

            return 0;
        }

        public void RemoveItem(Item item, int Quantity)
        {
            int RemainQuantity = Quantity;
            List<ItemStack> RemoveItems = new List<ItemStack>();

            foreach (ItemStack IStack in InventoryList)
            {
                if (IStack.Name == item.Name)
                {
                    if(IStack.StackSize >= RemainQuantity)
                    {
                        IStack.StackSize -= RemainQuantity;
                        return;
                    }
                    else
                    {
                        RemoveItems.Add(IStack);
                        RemainQuantity -= IStack.StackSize;
                    }
                }
            }

            if (RemoveItems.Count > 0)
                RemoveItems.ForEach((i) => { InventoryList.Remove(i); });
            
            if(RemainQuantity != 0) {
                DebugConsole.instance.log("Inventory: " + entity.name + " tried to remove " + Quantity + " " + item.Name + " when there was enough. still need to remove " + RemainQuantity);
            }                  
        }
    }
}
