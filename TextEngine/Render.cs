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

        public static float FPS { get; private set; }

        static int previousToolbarLen = 0;
        internal static bool RecalcBordersNextFrame = false;
        internal static bool ChangeScreenPosNextFrame = false;

        internal static Scale oldScale;

        public static string[] CurrentFrame { get; private set; }

        internal static void Redraw()
        {
            System.Diagnostics.Stopwatch s = new();
            s.Start();

            FrameCount++;

            if (Game.Screen.height <= -3)
                //Too low to render toolbar
                return;

            WriteToolbar();

            if (ChangeScreenPosNextFrame)
            {
                ChangeScreenPosNextFrame = false;
                ChangeScreenPos();
            }

            if (RecalcBordersNextFrame)
            {
                RecalcBordersNextFrame = false;
                RecalcBorders();
            }

            if (Game.Screen.width <= 0 || Game.Screen.height <= 0)
                //No more rendering to do, game has like no height/width
                return;

            //The following code looks weird and scary
            //What it does, is it looks at each object and each cell it takes up,
            //And adds it to it's specified frame. It also has a 2 dimensional array for colour data
            //So it can store the colour values.

            //The reason it doesn't add the colour string outright is because it sets the frame with indexing
            //and changing the value will mess it all up

            string[] frame = new string[Game.Screen.height];
            Console.CursorVisible = false;

            for (int i = 0; i < frame.Length; i++)
            {
                frame[i] = emptyBar;
            }

            //Stores colour data
            (Cell cell, uint renderOrder)[,] colours = new (Cell, uint)[Game.Screen.width, Game.Screen.height];

            for (int objNumber = 0; objNumber < Game.GameObjects.Count; objNumber++)
            {
                if (objNumber >= Game.GameObjects.Count)
                    continue;

                GameObject obj = Game.GameObjects[objNumber];

                if (obj is null)
                    continue;

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

                        if (obj.RenderOrder < colours[renderPos.X, renderPos.Y].renderOrder)
                            continue;

                        StringBuilder line = new(frame[renderPos.Y]);
                        line[renderPos.X] = obj.texture[i, j].Character;
                        frame[renderPos.Y] = line.ToString();

                        Cell cell = obj.texture[i, j];

                        if (cell.Highlight == Color.Default)
                            //Transparency
                            cell.Highlight = colours[renderPos.X, renderPos.Y].cell.Color;

                        colours[renderPos.X, renderPos.Y] = (cell, obj.RenderOrder);
                    }
                }
            }

            //At this point, we have an uncoloured frame, and a list of colour data
            //Simply merge them
            int drawAtX = Game.ScreenPos.X;

            string beforeFrame = drawAtX > 0 ? "|" : "";

            if (drawAtX > 0)
                drawAtX--;

            for (int i = 0; i < frame.Length; i++)
            {
                if (i + Game.ScreenPos.Y < 0)
                    continue;

                Console.SetCursorPosition(drawAtX < 0 ? 0 : drawAtX, i + Game.ScreenPos.Y);

                string currentFrame = GetLine(frame[i], colours, i) + "|";

                frame[i] = currentFrame;

                currentFrame = beforeFrame + currentFrame;

                if (drawAtX < 0 && drawAtX < -currentFrame.Length)
                    continue;

                if (drawAtX < 0)
                    currentFrame = currentFrame[Math.Abs(drawAtX)..];

                Console.Write(currentFrame);
            }
            //Write(frame, colours);

            CurrentFrame = frame;

            FPS = 1000f / s.ElapsedMilliseconds;

            Game.WaitUntilUnpause();
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

            Console.CursorTop = Game.Screen.height + 2 + Game.ScreenPos.Y;
            Console.CursorLeft = 0;
            Console.Write(Game.ToolBar);
        }

        private static string GetLine(string UncolouredFrame, (Cell cell, uint renderOrder)[,] ColourData, int Ypos)
        {
            //Merge colourData and the frame

            if (UncolouredFrame.Length != ColourData.GetLength(0))
                return "";

            string s = "";
            for (int i = 0; i < UncolouredFrame.Length; i++)
            {
                try
                {
                    s += GetColour(UncolouredFrame[i], ColourData[i, Ypos]);
                }
                catch (IndexOutOfRangeException)
                {
                    continue;
                }
            }

            return s;
        }

        static string GetColour(char input, (Cell cell, uint renderOrder) colourData)
        {
            string inputStr = input.ToString();
            inputStr = inputStr.Colourize(colourData.cell.Color, colourData.cell.Highlight, colourData.cell.Formatting);
            return inputStr;
        }

        /// <summary>
        /// Recalculates the edges of the screen
        /// </summary>
        private static void RecalcBorders()
        {
            if (oldScale.height > Game.Screen.height)
                Console.Clear();

            if (Game.Screen.height + Game.ScreenPos.Y < 0)
                return;

            Console.CursorTop = 0;
            Console.CursorLeft = 0;
            emptyBar = "";
            string dashesAtBottom = ""; //The ----- at the bottom of the screen

            if (Game.Screen.width <= 0 || Game.Screen.height < 0)
            {
                Console.Clear();
                return;
            }

            for (int i = 0; i < Game.Screen.width; i++)
            {
                emptyBar += " ";
                dashesAtBottom += "_";
            }

            string clearEdges = "";
            for (int i = Game.Screen.width; i < oldScale.width; i++)
            {
                clearEdges += " ";
            }

            int DrawAtPos = Game.ScreenPos.X;

            string startOfBottom = DrawAtPos > 0 ? "|" : "";

            if (DrawAtPos > 0)
                DrawAtPos--;

            string bottom = startOfBottom + dashesAtBottom + "|" + clearEdges; //|______|

            while (DrawAtPos < 0)
            {
                DrawAtPos++;
                bottom = bottom[1..];
                dashesAtBottom = dashesAtBottom[1..];

                if (bottom == "" || dashesAtBottom == "")
                    return;
            }

            Console.CursorLeft = DrawAtPos;
            Console.CursorTop = Game.Screen.height + Game.ScreenPos.Y;

            Console.Write(bottom);

            if (Game.ScreenPos.Y > 0)
            {
                Console.SetCursorPosition(DrawAtPos, Game.ScreenPos.Y - 1);
                Console.Write(" " + dashesAtBottom);
            }
        }

        private static void ChangeScreenPos()
        {
            RecalcBordersNextFrame = true;
            Console.Clear();
        }
    }
}
