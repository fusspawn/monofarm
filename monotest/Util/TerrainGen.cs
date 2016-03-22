using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace monotest.Util
{
    class TerrainGen
    {
        private static Random R = new Random();
        private static List<int> Unwalkables = new List<int>() { 0, 1, 7, 527}; 

        public static int TileForHeight(int x, int y, int Height)
        {
            if (Height < 50)
                return 7;
            if (Height < 75)
                return 6;
            if (Height < 190)
                return 5;

            if (Height < 200)
                return 8;

            return R.Next(0, 1);
        }

        public static int DecorationForTile(int x, int y, int Height)
        {
            if(Height > 128 && Height < 170)
                if (R.Next(100) < 3)
                    return 527;

            return -1;
        }

        public static bool IsWalkable(int Type)
        {
            return !Unwalkables.Contains(Type);
        }
    }
}
