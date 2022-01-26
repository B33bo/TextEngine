using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEngine
{
#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
    public struct Color
#pragma warning restore CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
    {
        #region Colours
        public static Color Red { get; } = new(255, 0, 0);
        public static Color Green { get; } = new(0, 255, 0);
        public static Color Blue { get; } = new(0, 0, 255);
        public static Color Magenta { get; } = new(255, 0, 255);
        public static Color Cyan { get; } = new(0, 255, 255);
        public static Color Yellow { get; } = new(255, 255, 0);
        public static Color White { get; } = new(255, 255, 255);
        public static Color Black { get; } = new(1, 1, 1);
        public static Color Default { get; } = new(0, 0, 0);
        #endregion

        public static bool ConsoleColoursAllowed;

        public byte R, G, B;

        public const int Foreground = 38;
        public const int Background = 48;

        const char start = '\u001b';
        public const string EndColour = "\u001b[0m";

        public Color(byte R, byte G, byte B)
        {
            this.R = R; this.G = G; this.B = B;
        }

        public Color(string HexCode)
        {
            if (HexCode.Length < 6)
            {
                R = 0; G = 0; B = 0;
            }

            if (HexCode[0] == '#')
                HexCode = HexCode[1..];

            if (HexCode.Length != 6)
            {
                R = 0; G = 0; B = 0;
            }

            string RedComponent = HexCode[..2];
            string GreenComponent = HexCode[2..4];
            string BlueComponent = HexCode[4..];

            try
            {
                R = Convert.ToByte(RedComponent, 16);
                G = Convert.ToByte(GreenComponent, 16);
                B = Convert.ToByte(BlueComponent, 16);
            }
            catch (FormatException)
            {
                R = 0; G = 0; B = 0;
            }
        }

        public new string ToString()
        {
            //To hexcode
            string rComponent = R.ToString("x").PadLeft(2, '0');
            string gComponent = G.ToString("x").PadLeft(2, '0');
            string bComponent = B.ToString("x").PadLeft(2, '0');

            return rComponent + gComponent + bComponent;
        }

        public string ToConsoleColor(int type)
        {
            //escseq[;foreground;"2";R;G;B"m"
            //escseq[ is a character that tells the console that this is a colour input
            //semi-colons are used for seperation
            //foreground is 38 and background is 48

            if (!ConsoleColoursAllowed)
                return "";

            if (!(type == 38 || type == 48))
                return "";

            return $"{start}[{type};2;{R};{G};{B}m";
        }

        public static string ToConsoleColor(Color foreground, Color background)
        {
            //escseq[;foreground;"2";R;G;B"m"
            //escseq[ is a character that tells the console that this is a colour input
            //semi-colons are used for seperation
            //foreground is 38 and background is 48

            return foreground.ToConsoleColor(Foreground) + background.ToConsoleColor(Background);
        }

        public static Color Random()
        {
            System.Random rnd = new();
            Color c = new();

            byte[] colourData = new byte[3];
            rnd.NextBytes(colourData);

            c.R = colourData[0];
            c.G = colourData[1];
            c.B = colourData[2];
            return c;
        }

        #region Operators
        public static Color operator +(Color a, Color b)
        {
            int R = a.R + b.R;
            int G = a.G + b.G;
            int B = a.B + b.B;

            R = Math.Clamp(R, 0, 255);
            G = Math.Clamp(G, 0, 255);
            B = Math.Clamp(B, 0, 255);

            return new((byte)R, (byte)G, (byte)B);
        }

        public static Color operator -(Color a, Color b)
        {
            int R = a.R - b.R;
            int G = a.G - b.G;
            int B = a.B - b.B;

            R = Math.Clamp(R, 0, 255);
            G = Math.Clamp(G, 0, 255);
            B = Math.Clamp(B, 0, 255);

            return new((byte)R, (byte)G, (byte)B);
        }

        public static Color operator +(Color a, int b)
        {
            int colour = Convert.ToInt32(a.ToString());
            colour += b;
            return new(colour.ToString("x"));
        }

        public static Color operator -(Color a, int b)
        {
            int colour = Convert.ToInt32(a.ToString());
            colour -= b;
            return new(colour.ToString("x"));
        }

        public static Color operator *(Color a, int b)
        {
            int colour = Convert.ToInt32(a.ToString());
            colour *= b;
            return new(colour.ToString("x"));
        }

        public static Color operator /(Color a, int b)
        {
            int colour = Convert.ToInt32(a.ToString());
            colour /= b;
            return new(colour.ToString("x"));
        }

        public static bool operator ==(Color a, Color b)
        {
            return a.R == b.R && a.G == b.G && a.B == b.G;
        }

        public static bool operator !=(Color a, Color b)
            => !(a == b);
        #endregion
    }

    static class ColourExtensions
    {
        public static string Colourize(this string s, Color foreground)
        {
            if (!Color.ConsoleColoursAllowed)
                return s;

            return foreground.ToConsoleColor(Color.Foreground) + s + Color.EndColour;
        }

        public static string Colourize(this string s, Color foreground, Color background)
        {
            if (!Color.ConsoleColoursAllowed)
                return s;

            return Color.ToConsoleColor(foreground, background) + s + Color.EndColour;
        }
    }

    internal static class ConsoleColourManager
    {
        private const int STD_OUTPUT_HANDLE = -11;
        private const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;

        [DllImport("kernel32.dll")]
        private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        public static void Enable()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var iStdOut = GetStdHandle(STD_OUTPUT_HANDLE);

                var enable = GetConsoleMode(iStdOut, out var outConsoleMode)
                             && SetConsoleMode(iStdOut, outConsoleMode | ENABLE_VIRTUAL_TERMINAL_PROCESSING);
            }


            if (Environment.GetEnvironmentVariable("NO_COLOR") == null)
            {
                Color.ConsoleColoursAllowed = true;
            }
            else
            {
                Color.ConsoleColoursAllowed = false;
            }
        }
    }
}
