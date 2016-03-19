using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace monotest.Util
{
    class TerrainGen
    {
        private static Random R = new Random();
        private static List<int> Unwalkables = new List<int>() { 18, 19, 6, 7, 8, 9 }; 

        public static int TileForHeight(int x, int y, int Height)
        {
            if (Height < 50)
                return 7;
            if (Height < 128)
                return 6;
            if (Height < 200)
                return 5;   

            return R.Next(0, 1);
        }

        public static int DecorationForTile(int x, int y)
        {
            if (R.Next(100) < 3)
                return 527;

            return -1;
        }
    }
}
