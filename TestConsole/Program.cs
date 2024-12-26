using System;
using System.Text;
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
            Console.WriteLine("Lorem ipsum dolor sit amet, consectetur {0} adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. " +
                "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. " +
                "Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. ", me);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 19; i++)
            {
                sb.Append("test ");
            }
            sb.Append("tests ");
            for (int i = 0; i < 3; i++)
            {
                sb.Append("test ");
            }
            Console.WriteLine(sb.ToString(), 0);

            sb.Clear();
            for (int i = 0; i < 20; i++)
            {
                sb.Append("tests ");
            }
            Console.WriteLine(sb.ToString(), 0);

            //string prefix = string.Join(" ", Enumerable.Repeat("test", 20));
            //Console.WriteLine(prefix + "   " + "{0}", me);
            //Console.WriteLine("*** {0} ***", me);
        }

    }
}
