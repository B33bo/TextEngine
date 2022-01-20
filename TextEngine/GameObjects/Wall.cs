using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEngine.GameObjects
{
    //Wall is only used for testing purposes
    internal class Wall : GameObject
    {
        public override void KeyPress(ConsoleKey key)
        {
            if (key == ConsoleKey.P)
                Scale += new Scale(1, 1);
        }

        public override void OnCollision(GameObject type, Vector2D displacement)
        { }

        public override void Update()
        { }
    }
}
