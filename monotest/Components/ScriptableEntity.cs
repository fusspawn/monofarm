using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monotest.Components
{
    public class ScriptableEntity : Entity
    {

        public ScriptableEntity(string name) : base(name)
        {
        }

        public Component GetComponentByName(string ComponentType)
        {
            foreach(Component Comp in this.components) {
                if (Comp.GetType().Name == ComponentType)
                    return Comp;
            }

            return null;
        }
    }
}
