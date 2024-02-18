using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JRB.ConsoleUtilities
{
    public sealed class ErrorMessage : IColorCodedItem
    {

        public ErrorMessage(string message)
        {
            this.Message = message;
        }

        public string Message { get; private set; }

        public ConsoleColor OutputColor => ConsoleColor.Red;

        public string GetInitialDisplayText() => Message;

        public string GetSubsequentDisplayText() => Message;

    }
}
