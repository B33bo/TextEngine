using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Useful link: https://stackabuse.com/how-to-print-colored-text-in-python/
//I know it's in python, but the rules apply for c#
namespace TextEngine
{
    public static class Colors
    {
        public const string Starter = "\x1b[";
        public const string Stopper = "\x1b[0;0m";

        public const string Red = "";

        public static string GetColor(int color)
        {
            if (color == 0)
                return "";

            return $"{Starter}38;5;{color}m";
        }

        public static string GetColors(int foreground, int background)
        {
            return $"{GetColor(foreground)}{GetBackground(background)}";
        }

        public static string GetBackground(int color)
        {
            if (color == 0)
                return "";

            return $"{Starter}48;5;{color}m";
        }
    }
}
