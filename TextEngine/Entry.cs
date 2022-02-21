using System;
using System.Collections.Generic;
using TextEngine.Demos;
using TextEngine.Colors;

namespace TextEngine.Debug
{
    //EntryPoint is only used for testing purposes.
    internal class EntryPoint
    {

        static void Main(string[] args)
        {
            Console.WriteLine(new Color(100, 127, 127).ToHSV());
            ConsoleColourManager.Enable();
            Console.ResetColor();

            Game.Screen = new(30, 15);

            GameObject glitchObject = new();
            glitchObject.Position = new(2, 2);
            glitchObject.texture = new Texture(new string[] { "d\r", "\re" });
            glitchObject.Scale = new(5, 5);

            Game.AddObject(glitchObject);

            Game.OnQuitGame += () =>
            { Console.WriteLine(DemoRecorder.Instance.Demo.ToString()); };
            Game.Start(new Demo(@"C:\Users\B33bo\Desktop\w.txt"));
            //Game.Start();
        }
    }
}