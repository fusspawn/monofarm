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
                       /(TileWidth + TileMargin);
            }
        }

        public int YTiles => Tex.Height
                             /(TileHeight + TileMargin);

        public Rectangle GetTileRect(int Index)
        {
            Rectangle Ret = new Rectangle();
            Ret.Width = TileWidth;
            Ret.Height = TileHeight;
            int xTiles = XTiles;

            var xPos = Index % xTiles;
            var yPos = Index / xTiles;

            Ret.X = (xPos * (TileWidth + TileMargin));
            Ret.Y = (yPos * (TileHeight + TileMargin));

            return Ret;
        }
    }
}