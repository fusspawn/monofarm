using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibNoise.Primitive;
using Newtonsoft.Json;
using LibNoise.Utils;
using Nez.Console;


namespace monotest.Util
{
    public static class Noise2d
    {

        public static int MaxX;
        public static int MaxY;
        public static ImprovedPerlin Perlin = new ImprovedPerlin();

        private static float NoiseScale =1f;
        static float min = float.MaxValue;
        static float max = float.MinValue;

        public static float NoiseAt(float x, float y)
        {
            return Perlin.GetValue(x, 0, y);
        }


        [Command("world-noise-scale","")]
        public static void SetWorldNoiseScale(float _NoiseScale)
        {
            DebugConsole.instance.log("NoiseSCALE =" + _NoiseScale.ToString());
            Noise2d.NoiseScale = _NoiseScale;
        }

        public static int[,] GenerateNoiseMap(int startx, int starty, int width, int height, int octaves, int maxrange)
        {
            
            DebugConsole.instance.log("StartX: " + startx + " StartY: " + starty);
            DebugConsole.instance.log("Width: " + width + " Height: " + height);

            var data = new float[width * height];
            var frequency = .5f;
            var amplitude = 1f;

            for (var octave = 0; octave < octaves; octave++)
            {
                for (var y = starty; y < starty + (height); y++)
                {
                    for (var x = startx; x < startx + (width); x++)
                    {
                        var i = x - startx;
                        var j = y - starty;
                        float normx = ((x * frequency)/ width) *NoiseScale;
                        float normy = ((y * frequency)/ height) *NoiseScale;
                        var noise = NoiseAt(normx, normy);
                        noise = data[j*width + i] += noise*amplitude;
                        min = Math.Min(min, noise);
                        max = Math.Max(max, noise);
                    }
                }

                frequency *= 2;
                amplitude /= 2;
            }
            

            var map = new int[width, height];
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    map[x, y] = (int) (((data[y*width + x] - min) / (max - min)) * maxrange);
                }
            }

            return map;
        }
       
    }
}
