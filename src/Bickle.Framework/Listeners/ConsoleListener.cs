using System;
using System.Collections.Generic;
using System.Linq;

namespace Bickle.Listeners
{
    public abstract class ConsoleListener : ITestResultListener
    {
        private readonly List<string> _failures = new List<string>();
        private readonly List<string> _ignored = new List<string>();
        private readonly List<string> _pendings = new List<string>();
        private int _successCount;
        private int _totalCount;

        #region ITestResultListener Members

        public void Success(IExample example)
        {
            _totalCount++;
            Write(MessageType.Succeeded, ".");
            _successCount++;
        }

        public void Ignored(IExample example)
        {
            _totalCount++;
            Write(MessageType.Ignored, "I");
            _ignored.Add((_ignored.Count + 1) + ") Ignored: " + example.FullName);
        }

        public void Failed(IExample example, Exception exception)
        {
            _totalCount++;
            Write(MessageType.Failure, "F");
            _failures.Add(CreateFailureMessage(example, exception));
        }

        public void Pending(IExample example)
        {
            _totalCount++;
            Write(MessageType.Pending, "P");
            _pendings.Add((_pendings.Count + 1) + ") Pending: " + example.FullName);
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

        #endregion

        protected abstract void Write(MessageType messageType, string message);

        private void WriteLine(string message)
        {
            WriteLine(MessageType.Normal, message);
        }

        private void WriteLine(MessageType messageType, string message)
        {
            Write(messageType, message + "\r\n");
        }

        private string CreateFailureMessage(IExample example, Exception exception)
        {
            const string fmt =
                @"{0}) Failed: {1}
{2}";
            return string.Format(fmt, _failures.Count + 1, example.FullName, GetExceptionMessage(exception));
        }

        private static string GetExceptionMessage(Exception exception)
        {
            if (exception is AssertionException)
                return "Failed: " + exception.Message;

            return exception.ToString();
        }

        private void WriteCount(string single, string plural, int count, MessageType nonZeroColor)
        {
            Write(MessageType.Normal, ", ");
            int failureCount = count;
            MessageType messageType = failureCount > 0 ? nonZeroColor : MessageType.Succeeded;

            if (failureCount == 1)
            {
                Write(messageType, "1 " + single);
            }
            else
            {
                Write(messageType, failureCount + " " + plural);
            }
        }

        private void WriteSpecInfos(string title, IEnumerable<string> infos, MessageType messageType)
        {
            if (!infos.Any())
                return;

            WriteLine("");
            WriteLine(messageType, title);
            foreach (string info in infos)
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