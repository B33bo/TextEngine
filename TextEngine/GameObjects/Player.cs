using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEngine.GameObjects
{
    //Player is only used for testing purposes
    internal class Player : GameObject
    {
        public override void KeyPress(ConsoleKey key)
        {
            Vector2D movement = new();

            if (key == ConsoleKey.W)
            {
                movement.Y-=1;
                Character = '^';
            }
            if (key == ConsoleKey.S)
            {
                movement.Y+=1;
                Character = 'V';
            }

            if (key == ConsoleKey.A)
            {
                movement.X -= 1;
                Character = '<';
            }
            if (key == ConsoleKey.D)
            {
                movement.X+=1;
                Character = '>';
            }

            if (key == ConsoleKey.Q)
            {
                Scale = new(Scale.width, Scale.height + 1);
            }
            if (key == ConsoleKey.E)
            {
                Scale = new(Scale.width + 1, Scale.height);
            }

            if (key == ConsoleKey.C)
                Color++;

            if (key == ConsoleKey.L)
                Game.ToolBar = "you said: " + Game.Ask();

            if (key == ConsoleKey.K)
                Game.Width += 2;

            Move(movement);
            if (key == ConsoleKey.Escape)
                Game.Stop();
        }

        public override void OnCollision(GameObject type, Vector2D Displacement)
        {
        }

        public override void Update()
        {
            //Game.ToolBar = $"{Position} {Render.AverageFPS,-10} {Game.CallsPerSecond}";
        }
    }
}
