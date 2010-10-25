using System;
using System.Collections.Generic;
using System.Linq;

namespace Bickle
{
    public class ConsoleListener : ITestResultListener
    {
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
            Console.Write("F");
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
            Console.Write(".");
            _successCount++;
        }

        public void Finished()
        {
            Console.WriteLine();

            var failureCount = _failures.Count;
            var message = _totalCount + " examples";
            if (failureCount == 1)
            {
                message += ", 1 failure";
            }
            else
            {
                message += ", " + failureCount.ToString() + " failures";
            }
            Console.WriteLine(message);

            WriteSpecInfos("Failures:", _failures);
            WriteSpecInfos("Pending:", _pendings);
        }

        private void WriteSpecInfos(string title, List<string> infos)
        {
            if (!infos.Any())
                return;

            Console.WriteLine();
            Console.WriteLine(title);
            foreach (var failure in infos)
            {
                Console.WriteLine();
                Console.WriteLine(failure);
            }
        }

        public void Pending(Example example)
        {
            Console.Write("P");
            _pendings.Add(example.FullName);
        }

        public void Ignored(Example example)
        {
            Console.Write("I");
        }
    }
}