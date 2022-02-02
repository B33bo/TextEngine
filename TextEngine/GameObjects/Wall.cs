using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEngine.GameObjects
{
    //Wall is only used for testing purposes
    internal class Wall : GameObject
    {
        public static Wall instance;

        static Texture texc;

        public Wall()
        {
            HasCollision = true;
            Color[] colors = new Color[] { Color.Default, Color.Blue };
            texc = new(
            new string[] { "ab", "cd", "ef", "gh" },
            null,
            null,
            new TextFormatting[,] { { TextFormatting.Underline, TextFormatting.Inverse }, { TextFormatting.Bold, TextFormatting.Inverse }, { TextFormatting.Bold, TextFormatting.Inverse }, { TextFormatting.Bold, TextFormatting.Inverse } });

            Scale = new(2, 4);
            texture = texc;
            Position = new(5, 5);
            //Character = '#';
            RenderOrder = 2;

            //Color = Color.Red;
        }

        public override void KeyPress(ConsoleKey key)
        {
            if (key == ConsoleKey.Spacebar)
                Scale += new Scale(1, 1);
        }

        public override void OnCollision(GameObject type, Vector2D displacement)
        { }

        public override void Update()
        { }
    }
}
