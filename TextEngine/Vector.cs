using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEngine
{
    public struct Vector2D
    {
        public int X, Y;
        public double Magnitude
        {
            get => Math.Sqrt(X * X + Y * Y);
        }

        public static Vector2D Zero { get => new(0, 0); }
        public static Vector2D One { get => new(1, 1); }

        public static Vector2D Up { get => new(0, -1); }
        public static Vector2D Down { get => new(0, 1); }
        public static Vector2D Left { get => new(-1, 0); }
        public static Vector2D Right { get => new(1, 0); }

        public Vector2D(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public static Vector2D operator +(Vector2D a, Vector2D b)
        {
            return new(a.X + b.X, a.Y + b.Y);
        }

        public static Vector2D operator -(Vector2D a, Vector2D b)
        {
            return new(a.X - b.X, a.Y - b.Y);
        }

        public static bool operator ==(Vector2D a, Vector2D b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(Vector2D a, Vector2D b)
        {
            return !(a == b);
        }

        public static Vector2D operator +(Vector2D a, Scale scale)
        {
            return new(a.X + scale.width - 1, a.Y + scale.height - 1);
        }

        public static Vector2D operator -(Vector2D a, Scale scale)
        {
            return new(a.X - (scale.width - 1), a.Y - (scale.height - 1));
        }

        public Vector2D ScreenWrap()
        {
            int newX = X;
            int newY = Y;

            return new(WrapInt(newX, Game.Screen.width), WrapInt(newY, Game.Screen.height));
        }

        public Vector2D ScreenClamp()
        {
            int newX = Math.Clamp(X, 0, Game.Screen.width - 1);
            int newY = Math.Clamp(Y, 0, Game.Screen.height - 1);

            return new(newX, newY);
        }

        public bool InCameraBounds()
        {
            Vector2D TopLeft = Camera.TopLeft;
            Vector2D BottomRight = Camera.BottomRight;

            if (X > Camera.Right || X < TopLeft.X)
                return false;

            if (Y < TopLeft.Y || Y > Camera.BottomRight.Y)
                return false;

            return true;
        }

        public bool InScreen()
        {
            if (X >= Game.Screen.width || X < 0)
                return false;

            if (Y < 0 || Y >= Game.Screen.height)
                return false;

            return true;
        }

        private static int WrapInt(int num, int max)
        {
            if (max <= 1)
                return 0;

            num %= max - 1;

            if (num < 0)
                num = max + num;

            return num;
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }

        public override bool Equals(object obj)
        {
            return (Vector2D)obj == this;
        }

        public static Vector2D Parse(string s)
        {
            try
            {
                if (s.StartsWith("(") && s.EndsWith(")"))
                    s = s[1..^1];

                string[] split = s.Split(',');
                return new(int.Parse(split[0]), int.Parse(split[1]));
            }
            catch (Exception)
            {
                throw new FormatException();
            }
        }

        public static bool TryParse(string s, out Vector2D result)
        {
            try
            {
                result = Parse(s);
                return true;
            }
            catch (FormatException)
            {
                result = new(0, 0);
                return false;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static Vector2D ScreenMiddleCenter
        {
            get => new(Console.WindowWidth / 2, Console.WindowHeight / 2);
        }

        public static Vector2D ScreenBottomRight
        {
            get => new(Console.WindowWidth, Console.WindowHeight);
        }
    }
}
