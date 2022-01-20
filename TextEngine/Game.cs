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
                Render.RecalcBordersNextFrame = true;
            }
        }

        public static int Height
        {
            get => height;
            set
            {
                height = value;
                Render.RecalcBordersNextFrame = true;
            }
        }

        public static float CallsPerSecond
        {
            get => GameLoopCalls / (stopwatch.ElapsedMilliseconds / 1000f);
        }

        public static ulong GameLoopCalls { get; private set; }

        private static bool Running = true;
        private static bool AskingQuestion = false;

        public static List<GameObject> gameObjects = new();

        public static Stopwatch stopwatch;
        public static event GameQuitHandler OnQuitGame;

        public static void Start()
        {
            stopwatch = new();

            RenderThread = new(() => { while (Running) Render.Redraw(); });
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

            Render.RecalcBorders();

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
                    GameLoopCalls++;
                    GameObject gm = gameObjects[i];
                    if (gm == null)
                        continue;
                    gm.Update();
                }

                WaitForAnswer();
            }
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
            stopwatch.Stop();
            AskingQuestion = true;
            string answer = Console.ReadLine();
            stopwatch.Start();

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

        internal static void WaitForAnswer()
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
