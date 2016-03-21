using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using monotest.Rendering;
using monotest.Util;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using IDrawable = monotest.Interfaces.IDrawable;
using IUpdateable = monotest.Interfaces.IUpdateable;

namespace monotest.Objects
{
    public class Player  
        : IUpdateable, IDrawable
    {
        public Texture2D PlayerTexture;
        public Vector2 Location;
        public float Rotation;
        public float Speed = 3f;

        public Player(int X, int Y)
        {
            Location = new Vector2(X * 16, Y * 16);

            MainGame.MainCamera.Pos = Location;
        }


        public void Update(GameTime T)
        {
            Vector2 NextLoc = Location;

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                NextLoc -= new Vector2(1, 0)*Speed;
                Rotation = MathHelper.ToRadians(180);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                NextLoc += new Vector2(1, 0)*Speed;
                Rotation = MathHelper.ToRadians(0);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                NextLoc -= new Vector2(0, 1)*Speed;
                Rotation = MathHelper.ToRadians(-90);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                NextLoc += new Vector2(0, 1)*Speed;
                Rotation = MathHelper.ToRadians(90);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.OemMinus))
                MainGame.MainCamera.Zoom -= 0.05f;


            if (Keyboard.GetState().IsKeyDown(Keys.OemPlus))
                MainGame.MainCamera.Zoom += 0.05f;

            if (Keyboard.GetState().IsKeyDown(Keys.R))
                MainGame.MainCamera.Zoom =1f;

            if (DebugManager.Display)
            {
                Vector2 loc = TilePosition;
                DebugManager.Values["PlayerXTile"] = ((int)loc.X).ToString();
                DebugManager.Values["PlayerYTile"] = ((int)loc.Y).ToString();
            }

            Location = NextLoc;
            MainGame.MainCamera.Pos = Location;
        }
        

        public void Draw(SpriteBatch B, GameTime T)
        {
            B.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, MainGame.MainCamera.GetTransformation(B.GraphicsDevice));
            B.Draw(PlayerTexture, Location, null, null,
                new Vector2(PlayerTexture.Width / 2, PlayerTexture.Height / 2), 
                Rotation, null, null, SpriteEffects.None, 0f);
            B.End();
        }

        public void LoadContent(ContentManager Manager)
        {
            PlayerTexture = Manager.Load<Texture2D>("Man Blue/manBlue_stand");
        }

        public Vector2 TilePosition
        {
            get { return Location/16; }
        }
    }
}
