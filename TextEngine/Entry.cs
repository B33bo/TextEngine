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
            Console.ResetColor();

            Game.Screen = new(10, 10);
            Player player = new();
            player.HasCollision = true;

            Wall wall = new();
            //wall.Texture = new(GetCell);

            //Wall.instance = wall;

            //Game.AddObject(wall);
            Game.AddObject(player);

            //Console.WriteLine((ConsoleKey)'\r');
            //new Demo.KeyPress('a'), new Demo.KeyPress('w'), new Demo.Loop()});
            Game.AddObject(new DemoRecorder());


            Game.OnQuitGame += () =>
            { Console.WriteLine(DemoRecorder.Instance.Demo.ToString()); };

            Game.Start(new Demo(@"C:\Users\B33bo\Desktop\ExampleDemo.txt"));
        }

        static void DASD()
        {
        }

        static Cell GetCell(int x, int y)
        {
            return new('#', Color.Red);
        }
    }
}
