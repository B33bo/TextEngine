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
        public Player()
        {
            Formatting = TextFormatting.Underline | TextFormatting.Inverse;
            HasCollision = true;
            //texture = new(new string[] { "c" }, new Color[,] { { Color.Default } }, new Color[,] { { Color.Red } });

        }

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
                movement.X += 1;
                Character = '>';
            }

            if (key == ConsoleKey.F)
            {
                Game.ToolBar += " " + Random.String(5);
            }

            if (key == ConsoleKey.C)
            {
                Game.Stop();
            }

            Move(movement);
        }

        public override void OnCollision(GameObject type, Vector2D Displacement)
        {
            
        }

        public override void Update()
        {
            //Game.ToolBar = $"{Position} {Render.FPS,-10} {Game.CallsPerSecond} {StressTest.items}";

        }
    }
}
