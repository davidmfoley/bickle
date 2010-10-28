using System;
using System.Collections.Generic;
using System.Reflection;
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
            AddHandler<ExampleElement>((e,explicitly) => new ExecuteElementTask(e.Id, explicitly));
            AddHandler<ExampleContainerElement>((e, explicitly) => new ExampleContainerTask(e.Id, explicitly));
            AddHandler<SpecElement>((e, explicitly) => new SpecTask(e.Id, e.AssemblyLocation, explicitly));
           
        }

        private LoadContextAssemblyTask GetLoadAssemblyTask(SpecElement e)
        {
            return new LoadContextAssemblyTask(e.AssemblyLocation);
        }

        private void AddHandler<T>(ElementHandler<T> func) where T : UnitTestElement
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

            var current = element;
            while (current != null)
            {
                foreach (var handler in _handlers)
                    handler(tasks, element, explicitElements.Contains(element));

                current = current.Parent;
            }

            if (element is SpecElement)
            {
                tasks.Add(new UnitTestTask(null, GetLoadAssemblyTask((SpecElement)element))); 
            }
            tasks.Add(new UnitTestTask(null, new LoadContextAssemblyTask(typeof(Spec).Assembly.Location)));
           
            tasks.Reverse();
                       
            return tasks;
        }
    }

    internal delegate void WrappedHandler(List<UnitTestTask> tasks, UnitTestElement element, bool explicitly);
  
    internal delegate RemoteTask ElementHandler<T>(T element, bool explicitly);
}