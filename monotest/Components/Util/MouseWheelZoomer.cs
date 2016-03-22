using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Nez;

namespace monotest.Components.Util
{
    class MouseWheelZoomer : Component, IUpdatable
    {
        public void update()
        {
            if (Input.isKeyPressed(Keys.OemPlus))
                entity.scene.camera.zoomIn(.1f);
            else if (Input.isKeyPressed(Keys.OemMinus))
            {
                entity.scene.camera.zoomOut(.1f);
            }
        }
    }
}
