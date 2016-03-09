using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace worldserver
{
    class Program
    {

        struct Vec2D
        {
            public int X;
            public int Y;
        }

        static void Main(string[] args)
        {
            Dictionary<Vec2D, int> map = new Dictionary<Vec2D, int>();
            Random r = new Random();
            for(var x = 0; x < 250000; x++)
                for (var y = 0; y < 250000; y++)
                    map[new Vec2D() {X= x, Y=y}] = r.Next(4);

            Console.WriteLine("World Ready");
            Console.ReadLine();
        }
    }
}
