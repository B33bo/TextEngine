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
            if (X > Camera.Right || X < Camera.Left)
                return false;

            if (Y < Camera.Top || Y > Camera.Bottom)
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

        public static Vector2D Random()
        {
            Random rnd = new();
            int x = rnd.Next(0, Game.Screen.width);
            rnd = new(rnd.Next());
            int y = rnd.Next(0, Game.Screen.height);

            return new(x, y);
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
    }
}
