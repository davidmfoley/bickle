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
        private List<string> _ignored = new List<string>();


        public void Success(Example example)
        {
            _totalCount++;
            Write(MessageType.Succeeded, ".");
            _successCount++;
        }

        public void Ignored(Example example)
        {
            _totalCount++;
            Write(MessageType.Ignored, "I");
            _ignored.Add((_ignored.Count + 1).ToString() + ") Ignored: " + example.FullName);
        }

        public void Failed(Example example, Exception exception)
        {
            _totalCount++;
            Write(MessageType.Failure, "F");
            _failures.Add(CreateFailureMessage(example, exception));
        }

        public void Pending(Example example)
        {
            _totalCount++;
            Write(MessageType.Pending, "P");
            _pendings.Add((_pendings.Count + 1).ToString() + ") Pending: " + example.FullName);
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

        public void Finished()
        {           
            WriteLine("");

            WriteSpecInfos("Failures:", _failures, MessageType.Failure);
            WriteSpecInfos("Pending:", _pendings, MessageType.Pending);
            WriteSpecInfos("Ignored:", _ignored, MessageType.Ignored);

            WriteLine("");

            Write(MessageType.Normal, _totalCount + " examples");

            WriteCount("failure", "failures", _failures.Count, MessageType.Failure);
            WriteCount("pending", "pending", _pendings.Count, MessageType.Pending);
            WriteCount("ignored", "ignored", _ignored.Count, MessageType.Ignored);

            WriteLine("");
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

        private void WriteSpecInfos(string title, List<string> infos, MessageType messageType)
        {
            if (!infos.Any())
                return;

            WriteLine("");
            WriteLine(messageType,title);
            foreach (var info in infos)
            {
                WriteLine("");
                WriteLine(messageType, info);
            }
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