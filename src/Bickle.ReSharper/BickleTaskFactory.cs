using System.Collections.Generic;
using JetBrains.ReSharper.UnitTestFramework;

namespace Bickle.ReSharper
{
    internal class BickleTaskFactory
    {
        public IList<UnitTestTask> GetTasks(UnitTestElement element, IList<UnitTestElement> explicitElements)
        {
            return new List<UnitTestTask>();
        }
    }
}