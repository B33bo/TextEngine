using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextEngine;

namespace TextEngine.Demos
{
    public class DemoRecorder : GameObject
    {
        public static DemoRecorder Instance { get; private set; }
        private Stopwatch stopwatch;
        public Demo Demo = new();
        public event DemoInputHandler InputRevieved;

        public delegate void DemoInputHandler(Demo.DemoInputType type);

        public DemoRecorder()
        {
            stopwatch = new();
            stopwatch.Start();
            Instance = this;

            Invisible = true;
            Character = 'D';
        }

        public override void KeyPress(ConsoleKey key)
        {
            Demo.Add(new Demo.Delay(stopwatch.ElapsedMilliseconds));
            TryInvokeNewDemoAdd(Demo[^1]);

            Demo.Add(new Demo.KeyPress(key));
            TryInvokeNewDemoAdd(Demo[^1]);
            stopwatch.Restart();
        }

        private void TryInvokeNewDemoAdd(Demo.DemoInputType type)
        {
            if (InputRevieved == null)
                return;

            InputRevieved.Invoke(type);
        }

        public static void TryAdd(Demo.DemoInputType input)
        {
            if (Instance is null)
                return;

            Instance.TryInvokeNewDemoAdd(input);
            Instance.Demo.Add(input);
        }
    }
}
