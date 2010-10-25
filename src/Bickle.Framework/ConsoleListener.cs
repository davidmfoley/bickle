using System;
using System.Collections.Generic;
using System.Linq;

namespace Bickle
{
    public class ColorConsoleListener : ConsoleListener
    {
        private ConsoleColor _defaultColor;
        private Dictionary<MessageType, ConsoleColor> _colors = new Dictionary<MessageType, ConsoleColor>();

        public ColorConsoleListener()
        {
            _defaultColor = Console.ForegroundColor;
            _colors.Add(MessageType.Failure, ConsoleColor.Red);
            _colors.Add(MessageType.Ignored, ConsoleColor.Gray);
            _colors.Add(MessageType.Pending, ConsoleColor.Yellow);

            _colors.Add(MessageType.Succeeded, ConsoleColor.Green);
        }

        protected override void Write(MessageType messageType, string message)
        {
            Console.ForegroundColor = GetColorForMessageType(messageType);
            Console.Write(message);
            Console.ForegroundColor = _defaultColor;
        }

        private ConsoleColor GetColorForMessageType(MessageType messageType)
        {
            if (_colors.ContainsKey(messageType))
                return _colors[messageType];

            return ConsoleColor.White;
        }
    }
    public abstract class ConsoleListener : ITestResultListener
    {
        protected abstract void Write(MessageType messageType, string message);

        void Write(string message)
        {
            Write(MessageType.Normal, message);
        }

        void WriteLine(string message)
        {
            WriteLine(MessageType.Normal, message);
        }

        private void WriteLine(MessageType messageType, string message)
        {
            Write(messageType, message + "\r\n");
        }

        private List<string> _failures = new List<string>();
        private int _successCount;
        private int _totalCount;
        private List<string> _pendings = new List<string>();

        public void Running(Example example)
        {
            _totalCount++;
        }

        public void Failed(Example example, Exception exception)
        {
            Write(MessageType.Failure, "F");
            _failures.Add(CreateFailureMessage(example, exception));
        }

        private string CreateFailureMessage(Example example, Exception exception)
        {
            const string fmt = 
                @"{0}) Failed: {1}
{2}";
            return string.Format(fmt, _failures.Count + 1, example.FullName, GetExceptionMessage(exception));
        }

        private string GetExceptionMessage(Exception exception)
        {
            if (exception is AssertionException)
                return "Failed: " + exception.Message;

            return exception.ToString();
        }

        public void Success(Example example)
        {
            Write(MessageType.Succeeded,".");
            _successCount++;
        }

        public void Finished()
        {
            WriteLine("");

          

            Write(MessageType.Normal, _totalCount + " examples");

            WriteCount("failure", "failures", _failures.Count, MessageType.Failure);
            WriteCount("pending", "pending", _pendings.Count, MessageType.Pending);

            WriteLine("");
           

            WriteSpecInfos("Failures:", _failures);
            WriteSpecInfos("Pending:", _pendings);
        }

        private void WriteCount(string single, string plural, int count, MessageType nonZeroColor)
        {
            Write(MessageType.Normal, ", ");
            var failureCount = count;
            var messageType = failureCount > 0 ? nonZeroColor : MessageType.Succeeded;

            if (failureCount == 1)
            {
                Write(messageType, "1 " + single);
            }
            else
            {
                Write(messageType, failureCount.ToString() + " " + plural);
            }
        }

        private void WriteSpecInfos(string title, List<string> infos)
        {
            if (!infos.Any())
                return;

            WriteLine("");
            WriteLine(title);
            foreach (var failure in infos)
            {
                WriteLine("");
                WriteLine(failure);
            }
        }

        public void Pending(Example example)
        {
            Write(MessageType.Pending, "P");
            _pendings.Add(example.FullName);
        }

        public void Ignored(Example example)
        {
            Write(MessageType.Ignored, "I");
        }
    }

    public enum MessageType
    {
        Failure,
        Normal,
        Pending,
        Ignored,
        Succeeded
    }
}