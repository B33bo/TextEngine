using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Useful link: https://stackabuse.com/how-to-print-colored-text-in-python/
//I know it's in python, but the rules apply for c# as long as you don't use octet for the numbers
namespace TextEngine
{
    public static class Colors
    {
        /// <summary>Start each string with this to make the console interpret it as a colour</summary>
        public const string Start = "\x1b[";

        /// <summary>This will cease having the funky colours</summary>
        public const string End = "\x1b[0;0m";

        public const string Red = "";

        /// <summary>Get the color from the specfic numbers (0-255)
        /// https://stackabuse.s3.amazonaws.com/media/how-to-print-colored-text-in-python-07.jpg </summary>
        /// <param name="color"></param>
        public static string GetColor(int color)
        {
            if (color == 0)
                return ""; //No colour

            //"38" means that it renderes the foreground, 5 is a constant idk what it does
            //Changing it makes the text go cool but weird
            return $"{Start}38;5;{color}m";
        }

        public static string GetColors(int foreground, int background)
        {
            return $"{GetColor(foreground)}{GetBackground(background)}";
        }

        public static string GetBackground(int color)
        {
            if (color == 0)
                return "";

            return $"{Start}48;5;{color}m";
        }
    }
}
