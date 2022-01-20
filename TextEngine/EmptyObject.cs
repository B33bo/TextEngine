using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextEngine;

namespace TextEngine
{
    //"EmptyObject" is a gameobject with nothing in it
    public class EmptyObject : GameObject
    {
        public EmptyObject()
        {
            Invisible = true;
        }

        public override void KeyPress(ConsoleKey key)
        { }

        public override void OnCollision(GameObject collision, Vector2D displacement)
        { }

        public override void Update()
        { }
    }
}
