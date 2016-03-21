using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using monotest.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;

namespace monotest.Components.World
{
    class ChunkRenderer
        : RenderableComponent
    {
        private ChunkDataComponent Data;


        private Color DebugColor;
        private RenderTarget2D RenderTexture;
        private bool TextureDirty = false;


        public override void onAddedToEntity()
        {
            Data = entity.getComponent<ChunkDataComponent>();
            
            base.onAddedToEntity();
        }

        public override void render(Graphics graphics, Camera camera)
        {
            throw new NotImplementedException();
        }

        }

    }
}
