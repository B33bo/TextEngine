using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEngine.Debug
{
    //Wall is only used for testing purposes
    internal class Wall : GameObject
    {
        public static Wall instance;

        static Texture texc;

        public Wall()
        {
            Character = '!';
            Position = RandomNG.Vector(Vector2D.Zero, Game.Screen);
        }

        public override void KeyPress(ConsoleKey key)
        {
            if (key == ConsoleKey.Spacebar)
                Position = RandomNG.Vector(Vector2D.Zero, Game.Screen);
        }
    }
}
