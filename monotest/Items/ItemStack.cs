using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monotest.Items
{
    public class ItemStack
    {
        public string Name;
        public int StackSize;

        public ItemStack(string name, int stacksize)
        {
            Name = name;
            StackSize = stacksize;
        }
    }
}
