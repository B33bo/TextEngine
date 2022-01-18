using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEngine.GameObjects
{
    class Coin : GameObject
    {
        public static int Coins;
        public override void KeyPress(ConsoleKey key)
        { }

        public override void OnCollision(GameObject type, Vector2D displacement)
        {
            if (type == Player.instance)
            {
                Coins++;
                Destroy();
                type.Position = new Vector2D(5, 5);
            }
        }

        public override void Update()
        { }
    }
}
