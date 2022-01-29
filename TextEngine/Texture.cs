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

                if (cells is null)
                    return new('?');

                x %= cells.GetLength(0); y %= cells.GetLength(1);
                return cells[x, y];
            }
            set => cells[x, y] = value;
        }

        public Func<int, int, Cell> CustomIndexer;

        public int Width => cells.GetLength(0);
        public int Height => cells.GetLength(1);

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

        public Texture(char s, Color color, Color background)
        {
            cells = new Cell[,] { { new Cell(s, color, background) } };
            CustomIndexer = null;
        }

        public Texture(string[] s, Color[,] colour)
        {
            cells = GetCells(s, colour, null);
            CustomIndexer = null;
        }

        public Texture(string[] s, Color[,] colour, Color[,] highlight)
        {
            cells = GetCells(s, colour, highlight);
            CustomIndexer = null;
        }

        public Texture(Func<int, int, Cell> CustomIndexer)
        {
            this.CustomIndexer = CustomIndexer;
            cells = null;
        }

        public void SetCellColor(int x, int y, Color colour)
        {
            cells[x, y].Color = colour;
        }

        public void SetCell(int x, int y, Cell cell)
        {
            cells[x, y] = cell;
        }

        Texture(Cell[,] c)
        {
            cells = c;
            CustomIndexer = null;
        }

        private static Cell[,] GetCells(string[] lines, Color[,] colour, Color[,] highlight)
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
                    Color Colour = UseColour ? colour[x, y] : Color.Default;
                    Color Highlight = UseHighlight ? highlight[x, y] : Color.Default;

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

        public void SetColor(Color color)
        {
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    cells[i, j].Color = color;
                }
            }
        }

        public void SetHighlight(Color color)
        {
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    cells[i, j].Highlight = color;
                }
            }
        }

        public static implicit operator Texture(Cell[,] c) => new(c);

        public static implicit operator Texture(string[] s) => new(s);
        public static implicit operator Texture(char s) => new(s);
    }

    public struct Cell
    {
        public char Character;
        public Color Color, Highlight;

        public Cell(char Char)
        {
            Character = Char;
            Color = Color.Default; Highlight = Color.Default;
        }

        public Cell(char Char, Color Color)
        {
            Character = Char;
            this.Color = Color;
            Highlight = Color.Default;
        }

        public Cell(char Char, Color Color, Color Highlight)
        {
            Character = Char;
            this.Color = Color;
            this.Highlight = Highlight;
        }

        public void SetColor(Color Color)
        {
            this.Color = Color;
        }
        public void SetHighlight(Color Highlight) => this.Highlight = Highlight;
        public void SetCharacter(char Character) => this.Character = Character;
    }
}
