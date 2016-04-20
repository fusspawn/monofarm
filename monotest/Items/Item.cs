using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monotest.Items
{
    public class Item
    {
        public string Name;
        public string Description;
        public int MaxStackSize;
        public bool Interactable = false;
        private Action<Item, Entity, Entity> OnInteractFunc;

        public void MakeInteractable(Action<Item, Entity, Entity> EntFunc)
        {
            Interactable = true;
            OnInteractFunc = EntFunc;
        }


        public void Interact(Entity Owner, Entity Target)
        {
            if (OnInteractFunc != null)
                OnInteractFunc(this, Owner, Target);
        }
    }
}
