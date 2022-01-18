using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextEngine;

namespace TextEngine
{
    public class Camera : GameObject
    {
        public static Camera Instance { get; private set; }

        public static int Left { get => Instance is null ? -1 : Instance.Position.X; }
        public static int Right { get => Instance is null ? -1 : Instance.Position.X + Game.Width - 1; }

        public static int Top { get => Instance is null ? -1 : Instance.Position.Y; }
        public static int Bottom { get => Instance is null ? -1 : Instance.Position.Y + Game.Height - 1; }

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
