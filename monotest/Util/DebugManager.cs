using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using IDrawable = monotest.Interfaces.IDrawable;
using IUpdateable = monotest.Interfaces.IUpdateable;

namespace monotest.Util
{
    class DebugManager : IUpdateable, IDrawable
    {
        public static SpriteFont Font; 
        public static Dictionary<string, string> Values = new Dictionary<string, string>();
        public static bool Display = false;

        public void Update(GameTime T)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.F12))
                Display = !Display;
        }

        public static Vector2 StartPos = new Vector2(20, 20);
        public static int YOffset = 15;


        public void Draw(SpriteBatch B, GameTime T)
        {
            if (!Display || Font == null)
                return;

            B.Begin();
            int index = 0;
            foreach (KeyValuePair<string, string> Pair in Values)
            {
                B.DrawString(Font, Pair.Key + ": " + Pair.Value, StartPos + new Vector2(0, YOffset * index), Color.White);
                index += 1;
            }
            B.End();

        }

        public void LoadContent(ContentManager Manager)
        {
           Font = Manager.Load<SpriteFont>("debugfont");
        }
    }
}
