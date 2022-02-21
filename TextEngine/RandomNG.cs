using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextEngine.Demos;
using TextEngine.Colors;

namespace TextEngine
{
    public static class RandomNG
    {
        private static bool InitialisedRandom = false;
        public static int Seed
        {
            get => _seed;
            internal set
            {
                if (InitialisedRandom)
                    return;

                InitialisedRandom = true;
                random = new(value);
                _seed = value;
                DemoRecorder.TryAdd(new Demo.RandomSeed(value));
            }
        }

        private static int _seed;
        private static Random random = new(0);
        private const string letters = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz1234567890";

        internal static void ResetRandom()
        {
            if (InitialisedRandom)
                return;
            Seed = new Random().Next();
        }

        public static int Int(int a, int b) =>
            random.Next(a, b);

        public static void Bytes(byte[] buffer) =>
            random.NextBytes(buffer);

        public static bool Bool() =>
            random.Next(0, 2) == 0;

        public static bool Bool(double Percentage)
            => random.NextDouble() < Percentage;

        public static Color Color()
        {
            Color c = new();

            byte[] colourData = new byte[3];
            random.NextBytes(colourData);

            c.R = colourData[0];
            c.G = colourData[1];
            c.B = colourData[2];
            return c;
        }

        public static double Percentage() =>
            random.NextDouble();

        public static string String(int length)
        {
            string str = "";
            for (int i = 0; i < length; i++)
                str += RandomNG.Choice(letters);
            return str;
        }

        public static string String(int length, char[] characters)
        {
            string str = "";
            for (int i = 0; i < length; i++)
                str += random.Next(RandomNG.Choice(characters));

            return str;
        }

        public static Vector2D Vector(Vector2D origin, Vector2D end) =>
            new(RandomNG.Int(origin.X, end.X), RandomNG.Int(origin.Y, end.Y));
        

        public static Scale Scale(Scale min, Scale max) =>
            new(RandomNG.Int(min.width, max.width), RandomNG.Int(min.height, max.height));

        public static T Choice<T>(T[] array) =>
            array[random.Next(array.Length)];
        
        public static T Choice<T>(List<T> list) =>
            list[random.Next(list.Count)];

        public static char Choice(string str) =>
            str[random.Next(str.Length)];
    }
}
