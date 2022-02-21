using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextEngine.Colors;

namespace TextEngine
{
    public class TextObject : GameObject
    {
        public TextObject(string text)
        {
            texture = new(text.Split('\n'));
            Scale = new(texture.Width, texture.Height);
        }

        public TextObject(string text, Color color)
        {
            texture = ColourObject(text.Split('\n'), color, Color.Default, TextFormatting.None);
            Scale = new(texture.Width, texture.Height);
        }

        public TextObject(string text, Color color, Color highlight)
        {
            texture = ColourObject(text.Split('\n'), color, highlight, TextFormatting.None);
            Scale = new(texture.Width, texture.Height);
        }

        public TextObject(string text, Color color, Color highlight, TextFormatting formatting)
        {
            texture = ColourObject(text.Split('\n'), color, highlight, formatting);
            Scale = new(texture.Width, texture.Height);
        }

        private static Texture ColourObject(string[] lines, Color color, Color highlight, TextFormatting formatting)
        {
            Texture texture = new(lines);

            for (int i = 0; i < texture.Width; i++)
            {
                for (int j = 0; j < texture.Height; j++)
                {
                    if (lines.Length <= j || lines[j].Length <= i)
                        continue;

                    texture.SetCellColor(i, j, color);
                    texture.SetCellHighlight(i, j, highlight);
                    texture.SetCellFormatting(i, j, formatting);
                }
            }
            return texture;
        }
    }
}
