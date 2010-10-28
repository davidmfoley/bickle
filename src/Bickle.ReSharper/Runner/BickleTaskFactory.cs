using System.Collections.Generic;
using Bickle.ReSharper.Provider.Elements;
using Bickle.ReSharper.Runner.Tasks;
using JetBrains.ReSharper.TaskRunnerFramework;
using JetBrains.ReSharper.UnitTestFramework;

namespace Bickle.ReSharper.Runner
{
    internal class BickleTaskFactory
    {
        private List<WrappedHandler> _handlers = new List<WrappedHandler>();
        public BickleTaskFactory()
        {
            SetHandler<ExampleElement>((e,explicitly) => new ExecuteElementTask(e.Id, explicitly));
            SetHandler<ExampleContainerElement>((e, explicitly) => new ExampleContainerTask(e.Id, explicitly));
            SetHandler<SpecElement>((e, explicitly) => new SpecTask(e.Id, explicitly));
        }

        private void SetHandler<T>(ElementHandler<T> func) where T : UnitTestElement
        {
            _handlers.Add((tasks, element, explicitly) =>
                {
                    if (!(element is T))
                        return;
                    var task = func((T)element, explicitly);
                    tasks.Add(new UnitTestTask(element, task));
                }
            );
        }

        public IList<UnitTestTask> GetTasks(UnitTestElement element, IList<UnitTestElement> explicitElements)
        {             
            var tasks = new List<UnitTestTask>();

            while (element != null)
            {
                foreach (var handler in _handlers)
                    handler(tasks, element, explicitElements.Contains(element));

                element = element.Parent;
            }

            tasks.Reverse();
                       
            return tasks;
        }
    }

    internal delegate void WrappedHandler(List<UnitTestTask> tasks, UnitTestElement element, bool explicitly);
  
    internal delegate RemoteTask ElementHandler<T>(T element, bool explicitly);
}