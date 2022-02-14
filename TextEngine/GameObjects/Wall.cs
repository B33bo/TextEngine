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
        public static Wall instance;

        static Texture texc;

        public Wall()
        {
            Character = '!';
            Position = Random.Vector(Vector2D.Zero, Game.Screen);
        }

        public override void KeyPress(ConsoleKey key)
        {
            if (key == ConsoleKey.Spacebar)
                Scale += new Scale(1, 1);
        }

        public override void OnCollision(GameObject type, Vector2D displacement)
        { }

        public override void Update()
        { }
    }
}
