using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEngine
{
    public class Render
    {
        public static ulong FrameCount { get; private set; }

        private static string emptyBar;

        public static float AverageFPS
        {
            get => FrameCount / (Game.stopwatch.ElapsedMilliseconds / 1000f);
        }

        static int previousToolbarLen = 0;
        internal static bool RecalcBordersNextFrame = false;

        internal static void Redraw()
        {
            FrameCount++;

            int newToolbarLen = Game.ToolBar.Length;

            while (previousToolbarLen > Game.ToolBar.Length)
            {
                Game.ToolBar += " ";
            }

            previousToolbarLen = newToolbarLen;

            if (Game.Height <= -3)
                return;

            Console.CursorTop = Game.Height + 2;
            Console.CursorLeft = 0;
            Console.Write(Game.ToolBar + " ");

            if (RecalcBordersNextFrame)
            {
                RecalcBordersNextFrame = false;
                RecalcBorders();
            }

            if (Game.Width <= 0 || Game.Height <= 0)
                return;

            string[] frame = new string[Game.Height];
            Console.CursorVisible = false;

            for (int i = 0; i < frame.Length; i++)
            {
                frame[i] = emptyBar;
            }

            string[,] colours = new string[Game.Width, Game.Height];
            foreach (GameObject obj in Game.gameObjects)
            {
                Vector2D drawPos = obj.Position;

                if (!drawPos.InCameraBounds())
                    continue;

                if (obj.Invisible)
                    continue;

                drawPos -= Camera.Instance.Position;

                StringBuilder line = new(frame[drawPos.Y]);
                line[drawPos.X] = obj.Character;
                frame[drawPos.Y] = line.ToString();
                colours[drawPos.X, drawPos.Y] = Colors.GetColors(obj.Color, obj.Highlight);
            }

            for (int i = 0; i < frame.Length; i++)
            {
                Console.CursorTop = i;
                Console.CursorLeft = 0;
                Console.Write(GetFrame(frame[i], colours, i));
            }

            Game.WaitForAnswer();
        }

        private static string GetFrame(string BaseFrame, string[,] ColourData, int index)
        {
            string s = "";
            for (int i = 0; i < BaseFrame.Length; i++)
            {
                bool isColour = ColourData[i, index] != "";

                if (isColour)
                    s += ColourData[i, index];

                s += BaseFrame[i];

                if (isColour)
                    s += Colors.Stopper;
            }

            return s;
        }

        internal static void RecalcBorders()
        {
            Console.Clear();
            Console.CursorTop = 0;
            Console.CursorLeft = 0;
            emptyBar = "";
            string dashesAtBottom = ""; //The ----- at the bottom of the screen

            if (Game.Width <= 0 || Game.Height <= 0)
                return;

            for (int i = 0; i < Game.Width; i++)
            {
                emptyBar += " ";
                dashesAtBottom += "-";
            }

            for (int i = 0; i < Game.Height; i++)
            {
                Console.WriteLine(emptyBar + "|");
            }
            Console.WriteLine(dashesAtBottom + "|");
        }
    }
}
