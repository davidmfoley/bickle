using System.Collections.Generic;
using Bickle.ReSharper.Provider.Elements;
using Bickle.ReSharper.Tasks;
using JetBrains.ReSharper.UnitTestFramework;

namespace Bickle.ReSharper
{
    internal class BickleTaskFactory
    {
        public IList<UnitTestTask> GetTasks(UnitTestElement element, IList<UnitTestElement> explicitElements)
        {
             
            var tasks = new List<UnitTestTask>();
            var example = element as ExampleElement;
            if (example != null)
                tasks.Add(new UnitTestTask(null, new  ExecuteElementTask(example.Id)));
            return tasks;
        }
    }
}