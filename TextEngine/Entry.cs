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
            Game.Screen = new(50, 50);
            Player player = new();
            player.HasCollision = true;

            Wall wall = new();
            wall.Color = "#FF0000";
            Wall wall2 = new();
            wall2.Color = "#FFFF00";

            Wall.instance = wall;
            wall2.RenderOrder = 0;

            Game.AddObject(player);

            Game.AddObject(wall);
            Game.AddObject(wall2);

            Game.OnQuitGame += () => Console.WriteLine("Ur trash");

            Game.Start();
        }

        static Cell GetCell(int x, int y)
        {
            x -= 20; y -= 20;
            string color = x * x + y * y == 100 ? "#FF0000" : "";
            return new Cell('-', color, "");
        }
    }
}
