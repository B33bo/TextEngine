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
            texture = new(new string[] { "c" }, new Color[,] { { Color.Default } }, new Color[,] { { Color.Red } });
        }

        public override void KeyPress(ConsoleKey key)
        {
            HasCollision = false;
            Vector2D movement = new();
            Scale movement2 = new();

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


            if (key == ConsoleKey.I)
            {
                movement2.height -= 1;
                Character = '^';
            }
            if (key == ConsoleKey.K)
            {
                movement2.height += 1;
                Character = 'V';
            }

            if (key == ConsoleKey.J)
            {
                movement2.width -= 1;
                Character = '<';
            }

            if (key == ConsoleKey.L)
            {
                movement2.width += 1;
                Character = '<';
            }

            if (key == ConsoleKey.P)
            {
                Game.Paused = !Game.Paused;
            }

            if (key == ConsoleKey.C)
            {
                //Color = Color.Random();
            }

            if (key == ConsoleKey.H)
            {
                //Highlight = Color.Random();
            }

            //if (key == ConsoleKey.J)
            //    Game.ScreenPos += new Vector2D(1, 1);

            //Game.ScreenPos += movement;
            //Game.Screen += movement2;

            Move(movement);
            //Camera.Instance.Position = Position;
            if (key == ConsoleKey.Escape)
                Game.Stop();
        }

        public override void OnCollision(GameObject type, Vector2D Displacement)
        {
        }

        public override void Update()
        {
            Game.ToolBar = $"{Position} {Render.FPS,-10} {Game.CallsPerSecond} {StressTest.items}";

        }
    }
}
