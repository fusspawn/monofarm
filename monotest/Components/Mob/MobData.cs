using monotest.Items;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monotest.Components.Mob
{
    public class MobData : Component
    {
        public string Name;
        public int Faction;
        public int Health;
        public int MaxHealth;

        public List<ItemStack> Inventory = new List<ItemStack>();
    }
}
