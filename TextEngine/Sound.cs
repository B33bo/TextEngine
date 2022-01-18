using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace TextEngine
{
    public class Sound
    {
        public static void PlayNotes(Note[] notes, bool Loop)
        {
            Thread soundThread = Loop ? new(PlayNotesForever) : new(PlayNotes);
            soundThread.Start(notes);
        }

        public static void PlayNotes(string Keys, bool Loop)
        {
            Note[] notes = new Note[Keys.Length];
            for (int i = 0; i < Keys.Length; i++)
            {
                notes[i] = Note.FromChar(Keys[i]);
            }

            PlayNotes(notes, Loop);
        }

        public static void PlayNote(Note note)
        {
            Thread soundThread = new(Play);
            soundThread.Start(note);
        }

        private static void PlayNotes(object noteObjs)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return;

            Note[] notes = (Note[])noteObjs;

            for (int i = 0; i < notes.Length; i++)
            {
                if (notes[i].Frequency < 37)
                {
                    Thread.Sleep(notes[i].Duration);
                    continue;
                }

                Console.Beep(notes[i].Frequency, notes[i].Duration);
            }
        }

        private static void PlayNotesForever(object noteObjs)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return;

            Note[] notes = (Note[])noteObjs;

            while (true)
            {
                for (int i = 0; i < notes.Length; i++)
                {
                    if (notes[i].Frequency < 37)
                    {
                        Thread.Sleep(notes[i].Duration);
                        continue;
                    }

                    Console.Beep(notes[i].Frequency, notes[i].Duration);
                }
            }
        }

        private static void Play(object noteObj)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return;

            Note note = (Note)noteObj;

            Console.Beep(note.Frequency, note.Duration);
        }
    }

    public struct Note
    {
        public int Frequency;
        public int Duration;

        public Note(int Frequency, int Duration)
        {
            this.Frequency = Frequency;
            this.Duration = Duration;
        }

        public static Note FromChar(char NoteChar)
        {
            char[] names = { 'c', 'd', 'e', 'f', 'g', 't', 'a', 'b', 'h', 'C', 'D', 'S', 'E', 'F', 'J', 'G', 'A', 'V', 'U', ' ' };
            int[] frequencies = { 262, 294, 330, 349, 392, 415, 440, 466, 494, 523, 587, 622, 659, 698, 740, 784, 880, 1047, 622, 0 };

            for (int i = 0; i < names.Length; i++)
            {
                if (names[i] == NoteChar)
                    return new(frequencies[i], 200);
            }
            return new(0, 0);
        }
    }
}
