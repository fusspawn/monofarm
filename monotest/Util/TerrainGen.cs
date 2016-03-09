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
            
            R = new Random(x * y);

            if (Height < 50)
                return R.Next(18, 19);
            if (Height < 128)
                return R.Next(4, 5);

            if (Height < 200)
                return R.Next(0 , 3);

            return R.Next(6, 9);
        }

        public static bool IsTileWalkable(int type)
        {
            return !Unwalkables.Contains(type);
        }
    }
}
