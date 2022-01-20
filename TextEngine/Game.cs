﻿using System;
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

        private static int width, height;

        //Use a property for width and height so it can change scale dynamically
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

        //It's like FPS but for how many calls the update methods recieve
        public static float CallsPerSecond
        {
            get => GameLoopCalls / (Timer.ElapsedMilliseconds / 1000f);
        }

        public static ulong GameLoopCalls { get; private set; }

        private static bool Running = true;
        private static bool AskingQuestion = false;

        public static List<GameObject> GameObjects
        {
            get; private set;
        } = new();

        public static Stopwatch Timer { get; private set; }
        public static event GameQuitHandler OnQuitGame;

        public static void Start()
        {
            Timer = new();

            //All render thread needs to do is call redraw
            RenderThread = new(() => { while (Running) Render.Redraw(); });

            //Input thread gets the user key press
            InputThread = new(() =>
            {
                while (Running)
                {
                    var KeyPress = Console.ReadKey(true).Key;
                    for (int i = 0; i < GameObjects.Count; i++)
                    {
                        GameObjects[i].KeyPress(KeyPress);
                    }
                    WaitForAnswer();
                }
            });

            //GameThread is used to call each object's update method
            GameThread = new(() => { while (Running) GameTick(); });

            Console.Clear();

            //Corners of screen
            Render.RecalcBorders();

            if (Camera.Instance is null)
                AddObject(new Camera());

            RenderThread.Start();
            InputThread.Start();
            GameThread.Start();

            Timer.Start();
        }

        private static void GameTick()
        {
            while (Running)
            {
                for (int i = 0; i < GameObjects.Count; i++)
                {
                    GameLoopCalls++;
                    GameObject gm = GameObjects[i];

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

            Console.CursorTop = height + 3;
            Console.CursorLeft = 0;
            Console.Write(spaces);

            AskingQuestion = false;
            return answer;
        }

        /// <summary>This method is called so you don't accidentally render something while
        /// asking a question</summary>
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
