using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEngine
{
    public struct Texture
    {
        public Cell[,] cells;

        public Cell this[int x, int y]
        {
            get
            {
                if (CustomIndexer is not null)
                    return CustomIndexer.Invoke(x, y);

                x %= cells.GetLength(0); y %= cells.GetLength(1);
                return cells[x, y];
            }
            set => cells[x, y] = value;
        }

        public Func<int, int, Cell> CustomIndexer;

        public Texture(string[] s)
        {
            cells = GetCells(s, null, null);
            CustomIndexer = null;
        }

        public Texture(char s)
        {
            cells = new Cell[,] { { new Cell(s) } };
            CustomIndexer = null;
        }

        public Texture(string[] s, string[,] colour)
        {
            cells = GetCells(s, colour, null);
            CustomIndexer = null;
        }

        public Texture(string[] s, string[,] colour, string[,] highlight)
        {
            cells = GetCells(s, colour, highlight);
            CustomIndexer = null;
        }

        public Texture(Func<int, int, Cell> CustomIndexer)
        {
            this.CustomIndexer = CustomIndexer;
            cells = null;
        }

        Texture(Cell[,] c)
        {
            cells = c;
            CustomIndexer = null;
        }

        private static Cell[,] GetCells(string[] lines, string[,] colour, string[,] highlight)
        {
            bool UseColour = colour != null;
            bool UseHighlight = highlight != null;

            int LengthOfCells = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Length > LengthOfCells)
                    LengthOfCells = lines[i].Length;
            }

            Cell[,] returnValue = new Cell[LengthOfCells, lines.Length];

            for (int y = 0; y < lines.Length; y++)
            {
                for (int x = 0; x < LengthOfCells; x++)
                {
                    string Colour = UseColour ? colour[x, y] : "";
                    string Highlight = UseHighlight ? highlight[x, y] : "";

                    if (lines[x].Length < LengthOfCells)
                    {
                        //Something like this occured
                        /**
                         * ###
                         * ##
                         * ###
                         * with a hole in it
                         */

                        returnValue[x, y] = new(' ', Colour, Highlight);
                        continue;
                    }

                    returnValue[x, y] = new(lines[x][y], Colour, Highlight);
                }
            }

            return returnValue;
        }

        public static implicit operator Texture(Cell[,] c) => new(c);

        public static implicit operator Texture(string[] s) => new(s);
        public static implicit operator Texture(char s) => new(s);
    }

    public struct Cell
    {
        public char Character;
        public string Color, Highlight;

        public Cell(char Char)
        {
            Character = Char;
            Color = ""; Highlight = "";
        }

        public Cell(char Char, string Color)
        {
            Character = Char;
            this.Color = Color;
            Highlight = "";
        }

        public Cell(char Char, string Color, string Highlight)
        {
            Character = Char;
            this.Color = Color;
            this.Highlight = Highlight;
        }
    }
}
