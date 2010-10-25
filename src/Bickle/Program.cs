using System;

namespace Bickle
{
    internal class Program
    {
        private const string Usage = "bickle {assembly to test}";

        private static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                PrintUsage();
                return;
            }

            string assemblyLocation = args[0];

            new SpecRunner().Run(assemblyLocation);
        }

        private static void PrintUsage()
        {
            Console.WriteLine(Usage);
        }
    }
}