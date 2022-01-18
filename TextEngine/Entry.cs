using System;
using TextEngine.GameObjects;

namespace TextEngine
{
    class EntryPoint
    {
        public static int Score = 0;
        static void Main(string[] args)
        {
            Game.Width = 10;
            Game.Height = 10;
            Player player = new();
            Game.AddObject(player);

            Game.Start();
        }
    }
}
