﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextEngine.Demos;

namespace TextEngine
{
    public static class Random
    {
        public static int Seed { get => _seed; set { random = new(value); _seed = value; DemoRecorder.TryAdd(new Demo.RandomSeed(value)); } }

        private static int _seed;
        private static System.Random random = new();
        private const string letters = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz1234567890";

        internal static void ResetRandom()
        {
            Seed = new System.Random().Next();
        }

        public static int Int(int a, int b)
        {
            return random.Next(a, b);
        }

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

        public static double Double()
        {
            return random.NextDouble();
        }

        public static string String(int length)
        {
            string str = "";
            for (int i = 0; i < length; i++)
            {
                str += Random.Choice(letters);
            }
            return str;
        }

        public static string String(int length, char[] characters)
        {
            string str = "";
            for (int i = 0; i < length; i++)
            {
                str += random.Next(Random.Choice(characters));
            }
            return str;
        }

        public static Vector2D Vector(Vector2D origin, Vector2D end)
        {
            return new(Random.Int(origin.X, end.X), Random.Int(origin.Y, end.Y));
        }

        public static Scale Scale(Scale min, Scale max)
        {
            return new(Random.Int(min.width, max.width), Random.Int(min.height, max.height));
        }

        public static T Choice<T>(T[] array)
        {
            return array[random.Next(array.Length)];
        }

        public static T Choice<T>(List<T> list)
        {
            return list[random.Next(list.Count)];
        }

        public static char Choice(string str)
        {
            return str[random.Next(str.Length)];
        }
    }
}