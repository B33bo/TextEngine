using System;
using System.Collections.Generic;
using TextEngine.GameObjects;
using TextEngine.Demos;

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
            ConsoleColourManager.Enable();

            Game.Screen = new(10, 10);
            Player player = new();
            player.HasCollision = true;

            //Wall wall = new();
            //wall.Texture = new(GetCell);

            //Wall.instance = wall;

            //Game.AddObject(wall);
            Game.AddObject(player);

            //Console.WriteLine((ConsoleKey)'\r');
            Console.ReadKey();
            //new Demo.KeyPress('a'), new Demo.KeyPress('w'), new Demo.Loop()});
            Game.Start();

            Game.AddObject(new DemoRecorder());

            Game.OnQuitGame += () => Console.WriteLine(DemoRecorder.Instance.Demo.ToString());
            //Game.Start();
        }

        static Cell GetCell(int x, int y)
        {
            return new('#', Color.Red);
        }
    }
}
