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
    //'Game' is used as a class to handle functions and stuff
    public static class Game
    {
        public static string ToolBar { get; set; } = "";

        private static Thread RenderThread;
        private static Thread InputThread;
        private static Thread GameThread;

        internal static int ThreadsRunning = 0;

        private static Scale scale;

        //Use a property for width and height so it can change scale dynamically
        public static Scale Screen
        {
            get => scale;
            set
            {
                Render.oldScale = scale;
                scale = value;
                Render.RecalcBordersNextFrame = true;
            }
        }

        private static Vector2D PositionAtScreen { get; set; }

        public static Vector2D ScreenPos
        {
            get => PositionAtScreen;
            set
            {
                PositionAtScreen = value;
                Render.ChangeScreenPosNextFrame = true;
            }
        }

        //It's like FPS but for how many calls the update methods recieve
        public static float CallsPerSecond
        {
            get; private set;
        }

        public static ulong GameLoopCalls { get; private set; }

        internal static bool Running = true;
        public static bool Paused
        {
            get => isPaused;
            set
            {
                isPaused = value;

                if (OnPauseGame != null)
                    OnPauseGame.Invoke();
            }
        }

        private static bool isPaused = false;
        private static bool AskingQuestion = false;

        public static List<GameObject> GameObjects
        {
            get; private set;
        } = new();

        public static Stopwatch Timer { get; private set; }
        public static event GameQuitHandler OnQuitGame;
        public static event GamePauseHandler OnPauseGame;

        public static void Start()
        {
            ConsoleColourManager.Enable();
            Console.OutputEncoding = new UnicodeEncoding();
            Timer = new();

            //All render thread needs to do is call redraw
            RenderThread = new(() => { ThreadsRunning++; while (Running) Render.Redraw(); ThreadsRunning--; });

            //Input thread gets the user key press
            InputThread = new(() =>
            {
                ThreadsRunning++;
                while (Running)
                {
                    var KeyPress = Console.ReadKey(true).Key;
                    int gameObjects = GameObjects.Count;
                    for (int i = 0; i < gameObjects; i++)
                    {
                        if (GameObjects.Count <= i)
                            //a gameobject got killed
                            break;
                        GameObjects[i].KeyPress(KeyPress);
                    }
                    WaitUntilUnpause();
                }
                ThreadsRunning--;
            });

            //GameThread is used to call each object's update method
            GameThread = new(() => { ThreadsRunning++; while (Running) GameTick(); ThreadsRunning--; });

            Console.Clear();

            //Corners of screen
            Render.RecalcBordersNextFrame = true;

            if (Camera.Instance is null)
                AddObject(new Camera());

            RenderThread.Start();
            InputThread.Start();
            GameThread.Start();

            Timer.Start();
            Running = true;
        }

        private static void GameTick()
        {
            while (Running)
            {
                Stopwatch timer = new();
                timer.Start();

                for (int i = 0; i < GameObjects.Count; i++)
                {
                    GameLoopCalls++;
                    GameObject gm = GameObjects[i];

                    if (gm == null)
                        continue;

                    gm.Update();
                }

                CallsPerSecond = 1000f / timer.ElapsedMilliseconds;
                WaitUntilUnpause();
            }
        }

        public static void Stop()
        {
            Running = false;

#pragma warning disable SYSLIB0006 // Type or member is obsolete
            try
            {
                GameThread.Abort();
                ThreadsRunning--;
            }
            catch (PlatformNotSupportedException)
            {
                GameThread = null;
                ThreadsRunning--;
            }
#pragma warning restore SYSLIB0006 // Type or member is obsolete

            while (ThreadsRunning != 0)
            {
                //Wait
            }

            Console.Clear();
            if (OnQuitGame != null)
                OnQuitGame.Invoke();
        }

        public static void AddObject(GameObject obj)
        {
            GameObjects.Add(obj);
        }

        /// <summary> Stop the game and ask a question</summary>
        public static string Ask()
        {
            Timer.Stop(); //so the FPS isn't incorrect
            AskingQuestion = true;
            string answer = Console.ReadLine();
            Timer.Start();

            //So you don't see your answer ages after
            string spaces = "";
            for (int i = 0; i < answer.Length; i++)
            {
                spaces += " ";
            }

            Console.CursorTop = Screen.height + 3;
            Console.CursorLeft = 0;
            Console.Write(spaces);

            AskingQuestion = false;
            return answer;
        }

        /// <summary>This method is called so you don't accidentally render something while
        /// asking a question</summary>
        internal static void WaitUntilUnpause()
        {
            if (AskingQuestion)
            {
                Console.CursorLeft = 0;
                Console.CursorTop = scale.height + 3;
                while (AskingQuestion) { }
            }

            while (Paused)
            {

            }
        }

        public delegate void GameQuitHandler();
        public delegate void GamePauseHandler();
    }
}
