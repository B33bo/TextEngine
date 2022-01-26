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
            HasCollision = false;
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
                Wall.instance.Scale += new Scale(0, 1);
                //Scale += new Scale(0, 1);
            }
            if (key == ConsoleKey.E)
            {
                //Scale += new Scale(1, 0);
                Wall.instance.Scale += new Scale(1, 0);
            }

            if (key == ConsoleKey.L)
                Game.ToolBar = "you said: " + Game.Ask();

            if (key == ConsoleKey.K)
                Game.Screen -= new Scale(0, 2);

            if (key == ConsoleKey.J)
                Game.Stop();

            //Move(movement);
            Camera.Instance.Position = Position;
            if (key == ConsoleKey.Escape)
                Game.Stop();
        }

        public override void OnCollision(GameObject type, Vector2D Displacement)
        {
        }

        public override void Update()
        {
            Game.ToolBar = $"{Position} {Render.FPS,-10} {Game.CallsPerSecond} {StressTest.items}";

            if (Game.Timer.ElapsedMilliseconds > 2000)
                Wall.instance.Destroy();
        }
    }
}
