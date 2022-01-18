using System;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEngine
{
    public static class Game
    {
        public static ulong FrameCount { get; private set; }
        public static string ToolBar = "";

        private static Thread RenderThread;
        private static Thread InputThread;
        private static Thread GameThread;

        private static int width, height;

        public static int Width
        {
            get => width;
            set
            {
                width = value;
                RecalcBordersNextFrame = true;
            }
        }

        public static int Height
        {
            get => height;
            set
            {
                height = value;
                RecalcBordersNextFrame = true;
            }
        }

        private static bool Running = true;
        private static bool AskingQuestion = false;

        public static List<GameObject> gameObjects = new();
        private static string emptyBar;

        static int previousToolbarLen = 0;

        public static Stopwatch stopwatch;
        public static event GameQuitHandler OnQuitGame;

        public static float AverageFPS
        {
            get => FrameCount / (stopwatch.ElapsedMilliseconds / 1000f);
        }

        public static void Start()
        {
            stopwatch = new();

            RenderThread = new(() => { while (Running) Redraw(); });
            InputThread = new(() =>
            { 
                while (Running)
                {
                    var KeyPress = Console.ReadKey(true).Key;
                    for (int i = 0; i < gameObjects.Count; i++)
                    {
                        gameObjects[i].KeyPress(KeyPress);
                    }
                    WaitForAnswer();
                }
            });
            GameThread = new(() => { while (Running) GameTick(); });

            Console.Clear();

            RecalcBorders();

            if (Camera.Instance is null)
                AddObject(new Camera());

            RenderThread.Start();
            InputThread.Start();
            GameThread.Start();

            stopwatch.Start();
        }

        private static void GameTick()
        {
            while (Running)
            {
                for (int i = 0; i < gameObjects.Count; i++)
                {
                    GameObject gm = gameObjects[i];
                    if (gm == null)
                        continue;
                    gm.Update();
                }

                WaitForAnswer();
            }
        }

        private static bool RecalcBordersNextFrame = false;
        private static void RecalcBorders()
        {
            Console.Clear();
            Console.CursorTop = 0;
            Console.CursorLeft = 0;
            emptyBar = "";
            string dashesAtBottom = ""; //The - at the bottom of the screen

            if (width <= 0 || height <= 0)
                return;

            for (int i = 0; i < width; i++)
            {
                emptyBar += " ";
                dashesAtBottom += "-";
            }

            for (int i = 0; i < height; i++)
            {
                Console.WriteLine(emptyBar + "|");
            }
            Console.WriteLine(dashesAtBottom + "|");
        }

        private static void Redraw()
        {
            FrameCount++;

            int newToolbarLen = ToolBar.Length;

            while (previousToolbarLen > ToolBar.Length)
            {
                ToolBar += " ";
            }

            previousToolbarLen = newToolbarLen;

            if (height <= -3)
                return;

            Console.CursorTop = height + 2;
            Console.CursorLeft = 0;
            Console.Write(ToolBar + " ");

            if (RecalcBordersNextFrame)
            {
                RecalcBordersNextFrame = false;
                RecalcBorders();
            }

            if (width <= 0 || height <= 0)
                return;

            string[] frame = new string[height];
            Console.CursorVisible = false;

            for (int i = 0; i < frame.Length; i++)
            {
                frame[i] = emptyBar;
            }

            foreach (GameObject obj in gameObjects)
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
            }

            for (int i = 0; i < frame.Length; i++)
            {
                Console.CursorTop = i;
                Console.CursorLeft = 0;
                Console.Write(frame[i]);
            }

            WaitForAnswer();
        }

        public static void Stop()
        {
            if (OnQuitGame != null)
                OnQuitGame.Invoke();

            Running = false;
        }

        public static void AddObject(GameObject obj)
        {
            gameObjects.Add(obj);
        }

        public static string Ask()
        {
            AskingQuestion = true;
            string answer = Console.ReadLine();

            string spaces = "";
            for (int i = 0; i < answer.Length; i++)
            {
                spaces += " ";
            }

            Console.CursorTop = height + 3;
            Console.CursorLeft = 0;
            Console.Write(spaces);

            AskingQuestion = false;
            return answer;
        }

        private static void WaitForAnswer()
        {
            if (!AskingQuestion)
                return;

            Console.CursorLeft = 0;
            Console.CursorTop = height + 3;

            while (AskingQuestion)
            {

            }
        }

        public delegate void GameQuitHandler();
    }
}
