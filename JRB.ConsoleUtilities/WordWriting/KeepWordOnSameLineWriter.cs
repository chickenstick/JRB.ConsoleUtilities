using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JRB.ConsoleUtilities.WordWriting
{
    public sealed class KeepWordOnSameLineWriter : IWordWriter
    {
        public void Write(string value)
        {
            bool startsWithWhiteSpace = StartsWithWhiteSpace(value);
            bool endsWithWhiteSpace = EndsWithWhiteSpace(value);

            if (startsWithWhiteSpace)
            {
                AddSpace();
            }

            List<string> words = SplitWords(value).ToList();
            for (int i = 0; i < words.Count; i++)
            {
                AddWord(words[i]);
                if (i < words.Count - 1)
                {
                    AddSpace();
                }
            }

            if (endsWithWhiteSpace)
            {
                AddSpace();
            }
        }

        public void Write(string format, object arg0)
        {
            string fmtd = string.Format(format, arg0);
            Write(fmtd);
        }

        private bool StartsWithWhiteSpace(string value)
        {
            return char.IsWhiteSpace(value[0]);
        }

        private bool EndsWithWhiteSpace(string value)
        {
            int lastIndex = value.Length - 1;
            return char.IsWhiteSpace(value[lastIndex]);
        }

        private IEnumerable<string> SplitWords(string value)
        {
            return value.Split(null).Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim());
        }

        private void AddSpace()
        {
            var pos = Console.GetCursorPosition();
            if (pos.Left > 0)
            {
                if (pos.Left < Console.WindowWidth - 1)
                {
                    Console.Write(" ");
                }
                else
                {
                    Console.WriteLine();
                }
            }
        }

        private void AddWord(string word)
        {
            var pos = Console.GetCursorPosition();
            int length = word.Length;
            if (pos.Left + length > Console.WindowWidth)
            {
                Console.WriteLine();
            }
            Console.Write(word);
        }

    }
}
