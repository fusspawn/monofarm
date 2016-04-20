using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using monotest.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;

namespace monotest.Components.Mob
{
    class CharacterRender
        : RenderableComponent
    {
        public SpriteSheet Sheet;
        public int Base = 0;
        public List<int> Extras = new List<int>();
         
        public override float width { get { return 16;  } }
        public override float height { get { return 16; } }

        public override void onAddedToEntity()
        {
            renderLayer = 1;
            Sheet = new SpriteSheet();
            Sheet.TileMargin = 1;
            Sheet.TileHeight = 16;
            Sheet.TileWidth = 16;
            Sheet.Tex = entity.scene.contentManager.Load<Texture2D>("roguelikeChar_transparent");

            base.onAddedToEntity();
        }

        public override void render(Graphics graphics, Camera camera)
        {
            graphics.batcher.draw(Sheet.Tex, entity.transform.position, Sheet.GetTileRect(Base), Color.White);
            foreach(var i in Extras)
                graphics.batcher.draw(Sheet.Tex, entity.transform.position, Sheet.GetTileRect(i), Color.White);
        }
    }
}
