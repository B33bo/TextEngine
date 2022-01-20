﻿using System;
using TextEngine.GameObjects;

namespace TextEngine
{
    //EntryPoint is only used for testing purposes.
    internal class EntryPoint
    {
        public static int Score = 0;
        static void Main(string[] args)
        {
            Game.Width = 10;
            Game.Height = 10;
            Player player = new();
            player.HasCollision = true;
            player.Invisible = true;

            Wall wall = new();
            wall.Character = '@';
            wall.Position = new(5, 4);
            wall.HasCollision = true;
            Wall wall2 = new();
            wall2.Character = '@';
            wall2.HasCollision = true;

            Game.AddObject(player);
            Game.AddObject(wall);

            Game.Start();
        }
    }
}
