using System;
using System.Collections.Generic;

namespace Bickle
{
    public class ConsoleListener : ITestResultListener
    {
        private List<string> _failures = new List<string>();
        private int _successCount;
        private int _totalCount;

        public void Running(It it)
        {
            _totalCount++;
        }

        public void Failed(It it, Exception exception)
        {
            Console.Write("F");
            _failures.Add(CreateFailureMessage(it, exception));

        }

        private string CreateFailureMessage(It it, Exception exception)
        {
            const string fmt = 
                @"{0}) Failed: {1}
{2}";
            return string.Format(fmt, _failures.Count + 1, it.FullName, exception);
        }

        public void Success(It it)
        {
            Console.Write(".");
            _successCount++;
        }

        public void Finished()
        {
            var failureCount = _failures.Count;
            var message = _totalCount + " examples";
            if (failureCount == 1)
            {
                message += "1 failure";
            }
            else
            {
                message += failureCount.ToString() + " failures";
            }
            Console.WriteLine(message);
        }
    }
}