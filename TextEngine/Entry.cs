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
            Wall.instance = wall;

            string grey = "555555";
            string orange = "FF8800";

            //Texture wallTexture = new(WallTex,
            //    new string[,] { { grey, grey, grey, grey }, { orange, grey, grey, orange }, { grey, orange, grey, grey }, { orange, grey, grey, grey } });

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
            string color = x * x == y * y ? "#FF0000" : "";
            return new Cell('-', color, "");
        }
    }
}
