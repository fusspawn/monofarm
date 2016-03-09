using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace monotest.Rendering
{
    public class SpriteSheet
    {
        public Texture2D Tex;

        public int TileHeight;
        public int TileMargin;
        public int TileWidth;

        public int XTiles
        {
            get
            {
                return Tex.Width
                       /(TileWidth + TileMargin*2);
            }
        }

        public int YTiles
        {
            get
            {
                return Tex.Height
                       /(TileHeight + TileMargin*2);
            }
        }

        public Rectangle GetTileRect(int Index)
        {
            Rectangle Ret = new Rectangle();
            Ret.Width = TileWidth;
            Ret.Height = TileHeight;

            var xPos = Index % XTiles;
            var yPos = Index / XTiles;

            Ret.X = (xPos * (TileWidth + TileMargin)) ;
            Ret.Y = (yPos * (TileHeight + TileMargin));

            return Ret;
        }
    }
}