using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEngine
{
    public struct Scale
    {
        public int width;
        public int height;

        public int Perimeter { get => 2 * (width + height); }

        public int Area { get => width * height; }

        public Vector2D[,] Intersections(Vector2D position)
        {
            Vector2D[,] intersections = new Vector2D[width, height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    intersections[i, j] = new Vector2D(width, height) + position;
                }
            }
            return intersections;
        }

        public Scale(int X, int Y)
        {
            width = X;
            height = Y;
        }

        public Scale(Vector2D scale)
        {
            width = scale.X;
            height = scale.Y;
        }

        public Scale(Vector2D origin, Vector2D destination)
        {
            Vector2D newSize = destination - origin;
            width = newSize.X;
            height = newSize.Y;
        }

        public Vector2D BottomRight(Vector2D pos)
        {
            return pos + this;
        }

        public static Scale operator +(Scale a, Scale b)
        {
            return new(a.width + b.width, a.height + b.height);
        }

        public static Scale operator -(Scale a, Scale b)
        {
            return new(a.width - b.width, a.height - b.height);
        }

        public static bool operator ==(Scale a, Scale b)
        {
            return a.width == b.width && a.height == b.height;
        }

        public static bool operator !=(Scale a, Scale b)
        {
            return !(a == b);
        }

        public static bool IntersectsWith((Vector2D topLeft, Vector2D BottomRight) a, (Vector2D topLeft, Vector2D BottomRight) b)
        {
            //One square is above the other
            if (a.topLeft.Y > b.BottomRight.Y || b.topLeft.Y > a.BottomRight.Y)
                return false;

            //One square is beside the other
            if (a.topLeft.X > b.BottomRight.X || b.topLeft.X > a.BottomRight.X)
                return false;

            return true;
        }
    }
}
