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
        private static readonly Dictionary<string, Type> _DemoInputTypes = new()
        {
            { "delay", typeof(Delay) },
            { "input", typeof(KeyPress) },
            { "question", typeof(Question) },
            { "loop", typeof(Loop) },
            { "seed", typeof(RandomSeed) },
            { "invalid", typeof(Invalid) },
        };

        internal static string DemoAnswer;
        internal static Demo Instance { get; private set; }
        private static int LastQuestionIndex = 0;
        private readonly List<DemoInputType> Inputs;

        public DemoInputType this[int index]
        {
            get => Inputs[index];
            set => Inputs[index] = value;
        }

        public DemoInputType this[Index index]
        {
            get => Inputs[index];
            set => Inputs[index] = value;
        }

        public void Add(DemoInputType input) => Inputs.Add(input);
        public void Remove(int index) => Inputs.RemoveAt(index);

        public Demo() => Inputs = new();
        public Demo(DemoInputType[] inputs) => Inputs = inputs.ToList();
        public Demo(List<DemoInputType> inputs) => Inputs = inputs;

        public Demo(string file)
        {
            string[] UnparsedDemo = File.ReadAllLines(file);
            Inputs = new ();

            for (int i = 0; i < UnparsedDemo.Length; i++)
            {
                Inputs.Add(new Invalid(i, "Unknown error", new Exception()));
                string[] Parameters = UnparsedDemo[i].Trim().Split(',');
                try
                {
                    string[][] ParametersToPass = { Parameters };
                    Inputs[i] = (DemoInputType)Activator.CreateInstance(_DemoInputTypes[Parameters[0].ToLower()], ParametersToPass);
                }
                catch (Exception e)
                {
                    Inputs[i] = new Invalid(i, $"{Parameters[0]} isn't a demo type", e);
                }
            }
        }

        public static void AddDemoType(string Name, Type type)
        {
            _DemoInputTypes.Add(Name.ToLower(), type);
        }

        internal string GetNextAnswer()
        {
            for (int i = LastQuestionIndex; i < Inputs.Count; i++)
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
            for (int i = 0; i < Inputs.Count; i++)
            {
                if (Inputs[i] is Loop)
                    i = 0;

                Inputs[i].OnCalled();
            }
        }

        public abstract class DemoInputType
        {
            public abstract void OnCalled();

            public DemoInputType() { }
        }

        public override string ToString()
        {
            string FinalString = "";
            for (int i = 0; i < Inputs.Count; i++)
            {
                FinalString += Inputs[i].ToString() + "\n";
            }
            return FinalString;
        }

        #region Input Types
        public class Delay : DemoInputType
        {
            public int Milliseconds;

            public Delay(long MS) => Milliseconds = (int)MS;

            public Delay(string[] args) =>
                Milliseconds = int.Parse(args[1]);

            public override void OnCalled()
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

            public KeyPress(string[] args) =>
                Key = (ConsoleKey)(args[1].ToUpper()[0]);


            public override void OnCalled()
            {
                Game.PressKey(Key);
            }

            public override string ToString() =>
                $"input,{Key}";
        }

        public class Question : DemoInputType
        {
            public string QuestionText = "";

            public Question(string question) =>
                QuestionText = question;

            public Question(string[] args) =>
                QuestionText = args[1];

            public override void OnCalled()
            { }

            public override string ToString() =>
                $"question,{QuestionText}";
        }

        public class Invalid : DemoInputType
        {
            int index;
            string ErrorMessage;
            Exception exception;

            public Invalid(int index, string ErrorMessage, Exception e)
            {
                this.index = index;
                this.ErrorMessage = ErrorMessage;
                this.exception = e;
            }

            public Invalid(string[] args)
            {
                index = int.Parse(args[1]);
                ErrorMessage = args[2];
            }

            public override void OnCalled()
            {
                //throw new InvalidDemoException($"AT: {index} " + ErrorMessage, exception);
            }

            public override string ToString() =>
                "invalid";
        }

        public class Loop : DemoInputType
        {
            public override void OnCalled() { }

            public Loop(string[] _) { }

            public override string ToString() =>
                "loop";
        }

        public class RandomSeed : DemoInputType
        {
            public int Seed;

            public RandomSeed(int Seed) =>
                this.Seed = Seed;

            public RandomSeed(string[] args) =>
                Seed = int.Parse(args[1]);

            public override void OnCalled() =>
                Random.Seed = Seed;

            public override string ToString() =>
                $"seed,{Seed}";
        }

        #endregion
    }
}
