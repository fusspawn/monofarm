using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using IUpdateable = monotest.Interfaces.IUpdateable;
using IDrawable = monotest.Interfaces.IDrawable;


namespace monotest.Objects
{
    public class ChunkStaticObject 
        : IUpdateable, IDrawable
    {
        public bool Walkable = false;

        public virtual void Update(GameTime T)
        {
        }

        public virtual void Draw(SpriteBatch B, GameTime T)
        {
        }

        public virtual void LoadContent(ContentManager Manager)
        {
        }
    }
}
