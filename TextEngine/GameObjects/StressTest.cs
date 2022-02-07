using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextEngine;

namespace TextEngine.GameObjects
{
    internal class StressTest : GameObject
    {
        static long lastMS;
        public static int items;
        static string[] tex =
        {
            "1234",
            "abcd",
            "qwer",
            "!\"$%",
        };

        public StressTest()
        {
            Position = Random.Vector(Camera.Instance.Position, Vector2D.ScreenBottomRight);
            items++;

            //Texture = new((x, y) => new Cell((char)(x + y + 97), Game.RandomColor()));

            Character = 'F';
        }

        public override void KeyPress(ConsoleKey key)
        {
            //rnd = new(rnd.Next());
            //Move(new (rnd.Next(-1, 1), rnd.Next(-1, 1)));

            if (key == ConsoleKey.Spacebar)
            {
                StressTest s = new();
                Game.AddObject(s);
            }
        }

        public override void OnCollision(GameObject collision, Vector2D displacement)
        {
            Color = Random.Color();
        }

        public override void Update()
        {
            if (Game.Timer.ElapsedMilliseconds - lastMS > 1000)
            {
                lastMS = Game.Timer.ElapsedMilliseconds;
                //Position = Vector2D.Random();
            }
            Color newCol = Random.Color();
            texture.SetCellColor(0, 0, newCol);
        }
    }
}
