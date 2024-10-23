using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace JRB.ConsoleUtilities.WordWriting
{
    public sealed class StandardWordWriter : IWordWriter
    {
        public void Write(string value)
        {
            Console.Write(value);
        }

        public void Write(string format, object arg0)
        {
            Console.Write(format, arg0);
        }
    }
}
