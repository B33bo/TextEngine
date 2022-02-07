using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextEngine;

namespace TextEngine
{
    //"Camera" is an empty marker for where the camera should be stored. Moving it will move the camera
    //Changing the scale has no effect. Use Game.Screen.width & Game.Screen.height
    public class Camera : GameObject
    {
        public static Camera Instance { get; private set; }

        public static int Right { get => Instance.Position.X + Game.Screen.width - 1; }

        private static int Bottom { get => Instance.Position.Y + Game.Screen.height - 1; }

        public static Vector2D TopLeft { get => Instance.Position; }

        public static Vector2D BottomRight { get => new(Right, Bottom); }

        public Camera()
        {
            Instance = this;
            Invisible = true;
            Character = '@';
            HasCollision = false;
            Position = new(0, 0);
        }

        public override void KeyPress(ConsoleKey key) { }

        public override void OnCollision(GameObject collision, Vector2D displacement) { }

        public override void Update() { }
    }
}
