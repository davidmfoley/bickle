using System;

namespace Bickle
{
    class Program
    {
        private const string Usage = "bickle {assembly to test}";

        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                PrintUsage();
                return;
            }

            var assemblyLocation = args[0];

        }

        private static void PrintUsage()
        {
            Console.WriteLine(Usage);
        }
    }
}
