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
        public List<Demo.DemoInputType> DemoInputs = new();

        public Demo Demo
        {
            get =>
                new(DemoInputs);
        }

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
            DemoInputs.Add(new Demo.Delay(stopwatch.ElapsedMilliseconds));
            DemoInputs.Add(new Demo.KeyPress(key));
            stopwatch.Restart();
        }

        public override void OnCollision(GameObject collision, Vector2D displacement) { }

        public override void Update() { }

        public static void TryAdd(Demo.DemoInputType input)
        {
            if (Instance is null)
                return;

            Instance.DemoInputs.Add(input);
        }
    }
}
