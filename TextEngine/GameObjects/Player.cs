using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEngine.GameObjects
{
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

            Move(movement);
            if (key == ConsoleKey.Escape)
                Game.Stop();
        }

        public override void OnCollision(GameObject type, Vector2D Displacement)
        {
        }

        public override void Update()
        {
            Game.ToolBar = $"{Position} {Render.AverageFPS,-10} {Game.CallsPerSecond}";
        }
    }
}
