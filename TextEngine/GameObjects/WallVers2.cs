using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEngine.GameObjects
{
    class WallVers2 : GameObject
    {
        public WallVers2()
        {
            Color = Color.Magenta;
            Character = 'M';
        }

        public override void KeyPress(ConsoleKey key)
        {
            
        }

        public override void OnCollision(GameObject collision, Vector2D displacement)
        {
            
        }

        public override void Update()
        {
            
        }
    }
}
