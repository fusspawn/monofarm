using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace monotest.Interfaces
{
    public interface IDrawable
    {
        void Draw(SpriteBatch B, GameTime T);
        void LoadContent(ContentManager Manager);
    }
}