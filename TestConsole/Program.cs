using System;

using JRB.ConsoleUtilities;

namespace TestConsole
{
    internal class Program
    {

        static void Main(string[] args)
        {
            using (ColorCodedConsoleOut consoleOut = new ColorCodedConsoleOut(keepWordsOnSameLine: true))
            {
                Console.SetOut(consoleOut);
                DisplayText();
            }
        }

        static void DisplayText()
        {
            Person me = new Person("Joe");
            //Console.WriteLine("Lorem ipsum dolor sit amet, consectetur {0} adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. " +
            //    "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. " +
            //    "Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. ", me);

            string prefix = string.Join(" ", Enumerable.Repeat("test", 20));
            Console.WriteLine(prefix + "   " + "{0}", me);
        }

    }
}
