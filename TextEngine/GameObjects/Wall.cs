using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEngine.GameObjects
{
    internal class Wall : GameObject
    {
        public override void KeyPress(ConsoleKey key)
        { }

        public override void OnCollision(GameObject type, Vector2D displacement)
        { }

        public override void Update()
        { }
    }
}
