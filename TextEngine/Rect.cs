using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEngine
{
    struct Rect
    {
        public Scale scale;
        public Vector2D origin;

        public int Left
        {
            get => origin.X;
            set => origin.X = value;
        }

        public int Top
        {
            get => origin.Y;
            set => origin.Y = value;
        }

        public int Right
        {
            get => origin.X + scale.width;
            set => scale.width = value - origin.X;
        }

        public int Bottom
        {
            get => origin.Y + scale.height;
            set => scale.height = value - origin.Y;
        }

        public static bool operator ==(Rect a, Rect b)
        {
            return a.scale == b.scale && a.origin == b.origin;
        }

        public static bool operator !=(Rect a, Rect b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            return (Rect)obj == this;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
