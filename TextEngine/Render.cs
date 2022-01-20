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
            get => FrameCount / (Game.Timer.ElapsedMilliseconds / 1000f);
        }

        static int previousToolbarLen = 0;
        internal static bool RecalcBordersNextFrame = false;

        internal static void Redraw()
        {
            FrameCount++;

            if (Game.Height <= -3)
                //Too low to render toolbar
                return;

            WriteToolbar();

            if (RecalcBordersNextFrame)
            {
                RecalcBordersNextFrame = false;
                RecalcBorders();
            }

            if (Game.Width <= 0 || Game.Height <= 0)
                //No more rendering to do, game has like no height/width
                return;

            //The following code looks weird and scary
            //What it does, is it looks at each object and each cell it takes up,
            //And adds it to it's specified frame. It also has a 2 dimensional array for colour data
            //So it can store the colour values.

            //The reason it doesn't add the colour string outright is because it sets the frame with indexing
            //and changing the value will mess it all up

            string[] frame = new string[Game.Height];
            Console.CursorVisible = false;

            for (int i = 0; i < frame.Length; i++)
            {
                frame[i] = emptyBar;
            }

            //Stores colour data
            string[,] colours = new string[Game.Width, Game.Height];

            foreach (GameObject obj in Game.GameObjects)
            {
                if (obj.Invisible)
                    continue;

                //The object position relative to the camera
                Vector2D drawPos = obj.Position - Camera.Instance.Position;

                for (int i = 0; i < obj.Scale.width; i++)
                {
                    for (int j = 0; j < obj.Scale.height; j++)
                    {
                        //Add the current part of the object to it's position. This is the Game cell we are drawing
                        Vector2D renderPos = new(drawPos.X + i, drawPos.Y + j);

                        if (!renderPos.InScreen())
                            //It's offscreen
                            continue;

                        StringBuilder line = new(frame[renderPos.Y]);
                        line[renderPos.X] = obj.Character;
                        frame[renderPos.Y] = line.ToString();

                        colours[renderPos.X, renderPos.Y] = Colors.GetColors(obj.Color, obj.Highlight);
                    }
                }
            }

            //At this point, we have an uncoloured frame, and a list of colour data
            //Simply merge them
            for (int i = 0; i < frame.Length; i++)
            {
                Console.CursorTop = i;
                Console.CursorLeft = 0;
                Console.Write(GetFrame(frame[i], colours, i));
            }

            Game.WaitForAnswer();
        }

        /// <summary>Writes the toolbar at the bottom of the screen</summary>
        private static void WriteToolbar()
        {
            //The reason it looks so complicated, is because of the way it is set up.
            //The rendering engine uses no Console.Clear() method as it is slow and obvious
            //This leaves the byproduct of having uncleared characters left over.

            //Example: Toolbar = "AAAAAA"; Toolbar = "BB";
            //Result: BBAAAA

            //To clear the remaining As, we add loads of spaces onto toolbar to make it "BB    "

            int newToolbarLen = Game.ToolBar.Length;

            while (previousToolbarLen > Game.ToolBar.Length)
            {
                Game.ToolBar += " ";
            }

            previousToolbarLen = newToolbarLen;

            Console.CursorTop = Game.Height + 2;
            Console.CursorLeft = 0;
            Console.Write(Game.ToolBar);
        }

        private static string GetFrame(string UncolouredFrame, string[,] ColourData, int Ypos)
        {
            //Merge colourData and the frame
            string s = "";
            for (int i = 0; i < UncolouredFrame.Length; i++)
            {
                bool isColour = ColourData[i, Ypos] != "";

                if (isColour)
                    s += ColourData[i, Ypos];

                s += UncolouredFrame[i];

                if (isColour)
                    s += Colors.End;
            }

            return s;
        }

        /// <summary>
        /// Recalculates the edges of the screen
        /// </summary>
        internal static void RecalcBorders()
        {
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
                Console.CursorTop = i;
                Console.WriteLine(emptyBar + "|");
            }
            Console.WriteLine(dashesAtBottom + "|");
        }
    }
}
