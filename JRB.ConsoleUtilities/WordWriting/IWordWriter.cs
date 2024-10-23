using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JRB.ConsoleUtilities.WordWriting
{
    public interface IWordWriter
    {
        void Write(string value);
        void Write(string format, object arg0);
    }
}
