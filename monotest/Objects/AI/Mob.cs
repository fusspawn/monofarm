using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using monotest.Interfaces;
using Microsoft.Xna.Framework.Content;
using IDrawable = monotest.Interfaces.IDrawable;
using IUpdateable = monotest.Interfaces.IUpdateable;

namespace monotest.Objects.AI
{
    public class Mob :
        IUpdateable,
        IDrawable
    {
        public Texture2D MobTex;
        public Vector2 Location;
        public Vector2 Destination;
        public float Rotation;


        public void Update(GameTime T)
        {
        }

        public void Draw(SpriteBatch B, GameTime T)
        {
            B.Begin();
            B.Draw(MobTex, Location, null, null,
                new Vector2(MobTex.Width / 2, MobTex.Height / 2),
                Rotation, null, null, SpriteEffects.None, 0f);
            B.End();
        }
        
        public void LoadContent(ContentManager Manager)
        {
            MobTex = Manager.Load<Texture2D>("manBlue_machine");
        }
    }
}
