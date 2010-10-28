using System;
using System.Collections.Generic;
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

        public BickleTaskRunner(IRemoteTaskServer server) : base(server)
        {
            SetHandler<ExecuteElementTask>(HandleExecuteElementTask);
            SetHandler<ExampleContainerTask>(HandleElementContainerTask);
            SetHandler<SpecTask>(HandleSpecTask);
        }

        private TaskResult HandleElementContainerTask(ExampleContainerTask exampleContainerTask, List<TaskExecutionNode> children)
        {
            
           Dispatch(children);
           return TaskResult.Success;
        }

        private TaskResult HandleSpecTask(SpecTask specTask, List<TaskExecutionNode> children)
        {           
            _currentSpec = (Spec) Activator.CreateInstance(Type.GetType(specTask.Id));
            
            Dispatch(children);
            return TaskResult.Success;
        }

        private TaskResult HandleExecuteElementTask(ExecuteElementTask executeElementTask, List<TaskExecutionNode> children)
        {           
            var example = (Example)_currentSpec.FindExample(executeElementTask.Id);
            example.Execute(_listener);
            return TaskResult.Success;
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