using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEngine.Demos
{
    public class Demo
    {
        internal static string DemoAnswer;
        internal static Demo Instance { get; private set; }
        private static int LastQuestionIndex = 0;
        private readonly DemoInputType[] Inputs;

        public Demo(DemoInputType[] inputs) => Inputs = inputs;
        public Demo(List<DemoInputType> inputs) => Inputs = inputs.ToArray();

        public Demo(string file)
        {
            string[] UnparsedDemo = File.ReadAllLines(file);
            Inputs = new DemoInputType[UnparsedDemo.Length];

            for (int i = 0; i < UnparsedDemo.Length; i++)
            {
                string[] Parameters = UnparsedDemo[i].Split(',');

                switch (Parameters[0].ToLower())
                {
                    default:
                        Inputs[i] = new Invalid();
                        break;
                    case "delay":
                        if (Parameters.Length < 2) { Inputs[i] = new Invalid(); break; }
                        if (!int.TryParse(Parameters[1], out int delay)) { Inputs[i] = new Invalid(); break; }

                        Inputs[i] = new Delay(delay);
                        break;
                    case "input":
                        if (Parameters.Length < 2) { Inputs[i] = new Invalid(); break; }

                        if (Parameters[1] == "enter")
                            Inputs[i] = new KeyPress(ConsoleKey.Enter);
                        else
                            Inputs[i] = new KeyPress(Parameters[1][0]);

                        break;
                    case "question":
                        if (Parameters.Length < 2) { Inputs[i] = new Invalid(); break; }
                        Inputs[i] = new Question(Parameters[1]);
                        break;
                    case "loop":
                        Inputs[i] = new Loop();
                        break;
                }
            }
        }

        internal string GetNextAnswer()
        {
            for (int i = LastQuestionIndex; i < Inputs.Length; i++)
            {
                if (Inputs[i] is Question)
                {
                    LastQuestionIndex = i + 1;
                    return (Inputs[i] as Question).QuestionText;
                }
            }
            return "";
        }

        public void Play()
        {
            Instance = this;
            DemoAnswer = GetNextAnswer();
            for (int i = 0; i < Inputs.Length; i++)
            {
                if (Inputs[i] is Loop)
                    i = 0;

                Inputs[i].OnPressed();
            }
        }

        public abstract class DemoInputType
        {
            public abstract void OnPressed();
        }

        public override string ToString()
        {
            string FinalString = "";
            for (int i = 0; i < Inputs.Length; i++)
            {
                FinalString += Inputs[i].ToString() + "\n";
            }
            return FinalString;
        }

        #region Input Types
        public class Delay : DemoInputType
        {
            public int Milliseconds;

            public Delay(long Ms) => Milliseconds = (int)Ms;

            public override void OnPressed()
            {
                Thread.Sleep(Milliseconds);
            }

            public override string ToString() =>
                $"delay,{Milliseconds}";
        }

        public class KeyPress : DemoInputType
        {
            public ConsoleKey Key;

            public KeyPress(ConsoleKey key) => Key = key;
            public KeyPress(char key) => Key = (ConsoleKey)(key.ToString().ToUpper()[0]);

            public override void OnPressed()
            {
                Game.PressKey(Key);
            }

            public override string ToString() =>
                $"input,{Key}";
        }

        public class Question : DemoInputType
        {
            public string QuestionText = "";

            public Question(string question) { QuestionText = question; }

            public override void OnPressed()
            { }

            public override string ToString() =>
                $"question,{QuestionText}";
        }

        public class Invalid : DemoInputType
        {
            public override void OnPressed() { }

            public override string ToString() =>
                "invalid";
        }

        public class Loop : DemoInputType
        {
            public override void OnPressed() { }

            public override string ToString() =>
                "loop";
        }

        #endregion
    }
}
