using System;
using System.Collections.Generic;
using System.Reflection;
using Bickle.ReSharper.Runner.Tasks;
using JetBrains.ReSharper.TaskRunnerFramework;

namespace Bickle.ReSharper.Runner
{
    internal delegate TaskResult RemoteTaskHandler(object task, List<TaskExecutionNode> children);

    internal delegate TaskResult RemoteTaskHandler<T>(T task, List<TaskExecutionNode> children);
 
    public class BickleTaskRunner : RecursiveRemoteTaskRunner
    {
        public static string RunnerId = "Bickle";
        private readonly Dictionary<Type,RemoteTaskHandler> _handlers = new Dictionary<Type, RemoteTaskHandler>();
        private ReSharperListener _listener;
        private Spec _currentSpec;
        private readonly AssemblyLoader _loader = new AssemblyLoader();

        public BickleTaskRunner(IRemoteTaskServer server) : base(server)
        {
            SetHandler<ExecuteElementTask>(HandleExecuteElementTask);
            SetHandler<ExampleContainerTask>(HandleElementContainerTask);
            SetHandler<SpecTask>(HandleSpecTask);
            SetHandler<LoadContextAssemblyTask>(HandleLoadAssemblyTask);
        }

        private TaskResult HandleLoadAssemblyTask(LoadContextAssemblyTask t, List<TaskExecutionNode> children)
        {
            //_loader.RegisterPath(t.AssemblyPath);
            
            Dispatch(children);
            return TaskResult.Success;
        }

        private TaskResult HandleElementContainerTask(ExampleContainerTask exampleContainerTask, List<TaskExecutionNode> children)
        {            
            Dispatch(children);
            var container = (ExampleContainer)_currentSpec.Find(exampleContainerTask.Id);

            return container.IsIgnored() ? TaskResult.Skipped : TaskResult.Success;
        }

        private TaskResult HandleSpecTask(SpecTask specTask, List<TaskExecutionNode> children)
        {
            var type = Assembly.LoadFrom(specTask.AssemblyLocation).GetType(specTask.Id);
            _currentSpec = (Spec) Activator.CreateInstance(type);
            
            Dispatch(children);
            return TaskResult.Success;
        }

        private TaskResult HandleExecuteElementTask(ExecuteElementTask executeElementTask, List<TaskExecutionNode> children)
        {           
            var example = (Example)_currentSpec.Find(executeElementTask.Id);
            example.Execute(_listener);
            return _listener.LastResult;
        }

        private void SetHandler<T>(RemoteTaskHandler<T> action) where T : RemoteTask
        {
            _handlers.Add(typeof (T), (task, children) => action((T)task, children));
        }

        public override void ExecuteRecursive(TaskExecutionNode node)
        {
            _listener = new ReSharperListener(Server);
            Dispatch(node);
        }

        private void Dispatch(IEnumerable<TaskExecutionNode> nodes)
        {
            foreach (var node in nodes)
            {
                Dispatch(node);
            }
        }

        private void Dispatch(TaskExecutionNode node)
        {
            var remoteTask = node.RemoteTask;


            Server.TaskStarting(remoteTask);
            _listener.CurrentTask = remoteTask;
            var handler = _handlers[remoteTask.GetType()];
            var result = handler(remoteTask, node.Children);
            Server.TaskFinished(remoteTask, result.ToString(), result);
        }

        private Example GetExample(string id)
        {
            return null;
        }

        public override TaskResult Start(TaskExecutionNode node)
        {
            return new TaskResult();
        }

        public override TaskResult Execute(TaskExecutionNode node)
        {
            return new TaskResult();
        }

        public override TaskResult Finish(TaskExecutionNode node)
        {
            return new TaskResult();
        }
    }
}