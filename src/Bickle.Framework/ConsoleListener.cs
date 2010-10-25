using System;
using System.Collections.Generic;

namespace Bickle
{
    public class ConsoleListener : ITestResultListener
    {
        private List<string> _failures = new List<string>();
        private int _successCount;
        private int _totalCount;

        public void Running(Example example)
        {
            _totalCount++;
        }

        public void Failed(Example example, Exception exception)
        {
            Console.Write("F");
            _failures.Add(CreateFailureMessage(example, exception));

        }

        private string CreateFailureMessage(Example example, Exception exception)
        {
            const string fmt = 
                @"{0}) Failed: {1}
{2}";
            return string.Format(fmt, _failures.Count + 1, example.FullName, exception);
        }

        public void Success(Example example)
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