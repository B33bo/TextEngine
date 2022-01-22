using System;
using TextEngine.GameObjects;

namespace TextEngine
{
    //EntryPoint is only used for testing purposes.
    internal class EntryPoint
    {
        public static int Score = 0;

        static string[] WallTex =
        {
            "####",
            "#OO#",
            "#O##",
            "O###",
        };

        static void Main(string[] args)
        {
            //Console.OutputEncoding = new System.Text.UnicodeEncoding();
            Console.WriteLine("\x1b[38;5;2mHELLO");
            Console.ReadLine();

            Game.Screen = new(50, 50);
            Player player = new();
            player.HasCollision = true;

            Wall wall = new();
            Wall.instance = wall;

            //Texture wallTexture = new(WallTex,
            //    new byte[,] { { 8, 8, 8, 8 }, { 8, 1, 1, 8 }, { 8, 1, 8, 8 }, { 1, 8, 8, 8 } },
            //    new byte[,] { { 0, 0, 0, 0 }, { 0, 208, 208, 0 }, { 0, 208, 0, 0 }, { 208, 0, 0, 0 } });

            Texture wallTexture = new(GetCell);

            wall.Texture = wallTexture;
            wall.Position = new(5, 4);
            wall.HasCollision = true;

            Game.AddObject(player);
            Game.AddObject(wall);

            Game.Start();
        }

        static Cell GetCell(int x, int y)
        {
            int n;
            if (y == 0)
                n = 0;
            else
                n = x % y;
            char newChar = (char)((n - 65 % (122 - 65)) + 65);
            return new(newChar, (byte)n, 0);
        }
    }
}
