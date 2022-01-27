using System;
using System.Collections.Generic;
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
            Game.Screen = new(10, 10);
            Player player = new();
            player.HasCollision = true;

            //Wall wall = new();
            //wall.Texture = new(GetCell);

            //Wall.instance = wall;

            //Game.AddObject(wall);
            Game.AddObject(player);

            Game.OnQuitGame += () => Console.WriteLine("Ur trash");


            Game.Start();
        }

        static Cell GetCell(int x, int y)
        {
            return new('#', Color.Red);
        }
    }
}
