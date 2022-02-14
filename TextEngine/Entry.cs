using System;
using System.Collections.Generic;
using TextEngine.GameObjects;
using TextEngine.Demos;
namespace TextEngine
{
    //EntryPoint is only used for testing purposes.
    internal class EntryPoint
    {

        static void Main(string[] args)
        {
            ConsoleColourManager.Enable();
            Console.ResetColor();

            Game.Screen = new(10, 10);

            Wall wall = new();

            Game.AddObject(new DemoRecorder());
            Game.AddObject(new Player());
            Game.AddObject(wall);


            Game.OnQuitGame += () =>
            { Console.WriteLine(DemoRecorder.Instance.Demo.ToString()); };
            Game.Start();
        }
    }
}