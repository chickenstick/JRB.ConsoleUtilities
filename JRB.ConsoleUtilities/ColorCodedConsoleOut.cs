using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JRB.ConsoleUtilities
{
    public class ColorCodedConsoleOut : StreamWriter
    {

        #region - Constants -

        private static readonly Regex _findTokenRegex = new Regex(@"(?<token>{\d+\:?.*?})", RegexOptions.Compiled);
        private static readonly Regex _getTokenPiecesRegex = new Regex(@"^{(?<index>\d+)(?<format>\:?.*)}$", RegexOptions.Compiled);

        #endregion

        #region - Constructor -

        public ColorCodedConsoleOut()
            : base(Console.OpenStandardOutput())
        {
            AutoFlush = true;
        }

        #endregion

        #region - Properties -

        public override Encoding Encoding => throw new NotImplementedException();

        #endregion

        #region - Overridden Methods -

        public override void Write(object? value)
        {
            if (value == null) 
                throw new ArgumentNullException(nameof(value));

            if (IsColorCoded(value))
            {
                ColorCodedWrite("{0}", value);
            }
            else
            {
                base.Write(value);
            }
        }

        public override void Write(string format, object? arg0)
        {
            if (arg0 == null)
                throw new ArgumentNullException(nameof(arg0));

            if (IsColorCoded(arg0))
            {
                ColorCodedWrite(format, arg0);
            }
            else
            {
                base.Write(format, arg0);
            }
        }

        public override void Write(string format, object? arg0, object? arg1)
        {
            if (arg0 == null)
                throw new ArgumentNullException(nameof(arg0));

            if (arg1 == null)
                throw new ArgumentNullException(nameof(arg1));

            if (IsColorCoded(arg0, arg1))
            {
                ColorCodedWrite(format, arg0, arg1);
            }
            else
            {
                base.Write(format, arg0, arg1);
            }
        }

        public override void Write(string format, object? arg0, object? arg1, object? arg2)
        {
            if (arg0 == null)
                throw new ArgumentNullException(nameof(arg0));

            if (arg1 == null)
                throw new ArgumentNullException(nameof(arg1));

            if (arg2 == null)
                throw new ArgumentNullException(nameof(arg2));

            if (IsColorCoded(arg0, arg1, arg2))
            {
                ColorCodedWrite(format, arg0, arg1, arg2);
            }
            else
            {
                base.Write(format, arg0, arg1, arg2);
            }
        }

        public override void Write(string format, params object?[] arg)
        {
            if (arg == null)
                throw new ArgumentNullException(nameof(arg));

            if (IsColorCoded(arg))
            {
                ColorCodedWrite(format, arg);
            }
            else
            {
                base.Write(format, arg);
            }
        }

        public override void WriteLine(object? value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (IsColorCoded(value))
            {
                ColorCodedWriteLine("{0}", value);
            }
            else
            {
                base.WriteLine(value);
            }
        }

        public override void WriteLine(string format, object? arg0)
        {
            if (arg0 == null)
                throw new ArgumentNullException(nameof(arg0));

            if (IsColorCoded(arg0))
            {
                ColorCodedWriteLine(format, arg0);
            }
            else
            {
                base.WriteLine(format, arg0);
            }
        }

        public override void WriteLine(string format, object? arg0, object? arg1)
        {
            if (arg0 == null)
                throw new ArgumentNullException(nameof(arg0));

            if (arg1 == null)
                throw new ArgumentNullException(nameof(arg1));

            if (IsColorCoded(arg0, arg1))
            {
                ColorCodedWriteLine(format, arg0, arg1);
            }
            else
            {
                base.WriteLine(format, arg0, arg1);
            }
        }

        public override void WriteLine(string format, object? arg0, object? arg1, object? arg2)
        {
            if (arg0 == null)
                throw new ArgumentNullException(nameof(arg0));

            if (arg1 == null)
                throw new ArgumentNullException(nameof(arg1));

            if (arg2 == null)
                throw new ArgumentNullException(nameof(arg2));

            if (IsColorCoded(arg0, arg1, arg2))
            {
                ColorCodedWriteLine(format, arg0, arg1, arg2);
            }
            else
            {
                base.WriteLine(format, arg0, arg1, arg2);
            }
        }

        public override void WriteLine(string format, params object?[] arg)
        {
            if (arg == null)
                throw new ArgumentNullException(nameof(arg));

            if (IsColorCoded(arg))
            {
                ColorCodedWriteLine(format, arg);
            }
            else
            {
                base.WriteLine(format, arg);
            }
        }

        #endregion

        #region - Private Methods -

        private bool IsColorCoded(object arg0) => arg0 is IColorCodedItem;
        private bool IsColorCoded(object arg0, object arg1) => IsColorCoded(arg0) || IsColorCoded(arg1);
        private bool IsColorCoded(object arg0, object arg1, object arg2) => IsColorCoded(arg0) || IsColorCoded(arg1) || IsColorCoded(arg2);
        private bool IsColorCoded(object[] arg) => arg.Any(IsColorCoded);

        private void ColorCodedWrite(string format, params object[] args)
        {
            List<IConsoleWriterBase> allWriters = GetConsoleWriters(format, args).ToList();
            foreach (IConsoleWriterBase consoleWriter in allWriters)
            {
                consoleWriter.WriteToConsole();
            }
        }

        private void ColorCodedWriteLine(string format, params object[] args)
        {
            ColorCodedWrite(format, args);
            Console.WriteLine();
        }

        private IEnumerable<IConsoleWriterBase> GetConsoleWriters(string format, object[] args)
        {
            int currentIndex = 0;
            List<CompositeFormatToken> allTokens = FindCompositeFormatTokens(format).OrderBy(t => t.Index).ToList();
            Dictionary<int, bool> argIndexesUsed = new Dictionary<int, bool>();
            foreach (CompositeFormatToken token in allTokens)
            {
                string preamble = format.Substring(currentIndex, token.Index - currentIndex);
                if (!string.IsNullOrEmpty(preamble))
                {
                    yield return new StandardConsoleWriter(preamble);
                }

                if (!argIndexesUsed.ContainsKey(token.ArgIndex))
                {
                    argIndexesUsed.Add(token.ArgIndex, false);
                }

                currentIndex = token.Index;
                object obj = args[token.ArgIndex];
                if (IsColorCoded(obj))
                {
                    bool alreadyUsed = argIndexesUsed[token.ArgIndex];
                    yield return new ColorCodedTokenConsoleWriter(token, (IColorCodedItem)obj, alreadyUsed);
                    argIndexesUsed[token.ArgIndex] = true;
                }
                else
                {
                    yield return new TokenConsoleWriter(token, obj);
                }

                currentIndex += token.RawText.Length;
            }

            if (currentIndex < format.Length)
            {
                string remainder = format.Substring(currentIndex, format.Length - currentIndex);
                yield return new StandardConsoleWriter(remainder);
            }
        }

        private IEnumerable<CompositeFormatToken> FindCompositeFormatTokens(string format)
        {
            MatchCollection col = _findTokenRegex.Matches(format);
            foreach (Match match in col)
            {
                Match pieceMatch = _getTokenPiecesRegex.Match(match.Value);
                string indexPiece = pieceMatch.Groups["index"].Value;
                string formatPiece = pieceMatch.Groups["format"].Value;

                int index = int.Parse(indexPiece);
                string f = (formatPiece ?? "").Trim();
                yield return new CompositeFormatToken(match.Index, match.Value, index, f);
            }
        }

        #endregion

        #region - Nested Classes -

        private class CompositeFormatToken
        {
            public CompositeFormatToken(int index, string rawText, int argIndex, string formatPiece)
            {
                this.Index = index;
                this.RawText = rawText;
                this.ArgIndex = argIndex;
                this.FormatPiece = formatPiece;
            }

            public int Index { get; private set; }
            public string RawText { get; private set; }
            public int ArgIndex { get; private set; }
            public string FormatPiece { get; private set; }

            public string ToArgIndexZero()
            {
                StringBuilder builder = new StringBuilder("{0");
                if (!string.IsNullOrWhiteSpace(FormatPiece))
                {
                    builder.Append(FormatPiece);
                }
                builder.Append("}");
                return builder.ToString();
            }

        }

        private interface IConsoleWriterBase
        {
            void WriteToConsole();
        }

        private class StandardConsoleWriter : IConsoleWriterBase
        {

            private string _rawText;

            public StandardConsoleWriter(string rawText)
            {
                _rawText = rawText;
            }

            public void WriteToConsole()
            {
                Console.Write(_rawText);
            }

        }

        private class TokenConsoleWriter : IConsoleWriterBase
        {

            private CompositeFormatToken _token;
            private object _arg;

            public TokenConsoleWriter(CompositeFormatToken token, object arg)
            {
                _token = token;
                _arg = arg;
            }

            public void WriteToConsole()
            {
                string token = _token.ToArgIndexZero();
                Console.Write(token, _arg);
            }

        }

        private class ColorCodedTokenConsoleWriter : IConsoleWriterBase
        {

            private CompositeFormatToken _token;
            private IColorCodedItem _arg;
            private bool _alreadyUsedArg;

            public ColorCodedTokenConsoleWriter(CompositeFormatToken token, IColorCodedItem arg, bool alreadyUsedArg)
            {
                _token = token;
                _arg = arg;
                _alreadyUsedArg = alreadyUsedArg;
            }

            public void WriteToConsole()
            {
                string token = _token.ToArgIndexZero();
                Console.ForegroundColor = _arg.OutputColor;
                string text = _alreadyUsedArg ? _arg.GetSubsequentDisplayText() : _arg.GetInitialDisplayText();
                Console.Write(token, text);
                Console.ResetColor();
            }

        }

        #endregion

    }
}
