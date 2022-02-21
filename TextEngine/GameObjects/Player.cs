using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextEngine;

namespace TextEngine.Debug
{
    //Player is only used for testing purposes
    internal class Player : GameObject
    {
        public Player Instance { get; private set; }
        public Player()
        {
            Instance = this;
            Position = Vector2D.GameMiddleCenter;
            Character = 'O';
            RenderOrder = 1;
        }

        public override void KeyPress(ConsoleKey key)
        {
            Vector2D movement;
            movement = key switch
            {
                ConsoleKey.W => Vector2D.Up,
                ConsoleKey.A => Vector2D.Left,
                ConsoleKey.S => Vector2D.Down,
                ConsoleKey.D => Vector2D.Right,
                _ => Vector2D.Zero,
            };

            if (key == ConsoleKey.C)
                Game.Stop();

            Move(movement);
        }
    }
}
