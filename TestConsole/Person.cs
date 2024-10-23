using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using JRB.ConsoleUtilities;

namespace TestConsole
{
    internal class Person : IColorCodedItem
    {

        public Person(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }

        public ConsoleColor OutputColor => ConsoleColor.Red;

        public string GetInitialDisplayText()
        {
            return $"<***{Name.ToUpper()}***>";
        }

        public string GetSubsequentDisplayText()
        {
            return GetInitialDisplayText();
        }
    }
}
