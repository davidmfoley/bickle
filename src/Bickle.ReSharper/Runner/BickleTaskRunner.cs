using System;
using System.Collections.Generic;
using Bickle.ReSharper.Tasks;
using JetBrains.ReSharper.TaskRunnerFramework;

namespace Bickle.ReSharper.Runner
{
    internal delegate TaskResult RemoteTaskHandler(object task, List<TaskExecutionNode> children);

    internal delegate TaskResult RemoteTaskHandler<T>(T task, List<TaskExecutionNode> children);
 
    public class BickleTaskRunner : RecursiveRemoteTaskRunner
    {
        public static string RunnerId = "Bickle";
        private readonly Dictionary<Type,RemoteTaskHandler> _handlers = new Dictionary<Type, RemoteTaskHandler>();
        private ITestResultListener _listener = new ReSharperListener();

        public BickleTaskRunner(IRemoteTaskServer server) : base(server)
        {
            SetHandler<ExecuteElementTask>(HandleExecuteElementTask);
            SetHandler<ElementContainerTask>(HandleElementContainerTask);
            SetHandler<SpecTask>(HandleSpecTask);
        }

        private TaskResult HandleElementContainerTask(ElementContainerTask elementContainerTask, List<TaskExecutionNode> children)
        {
            
           Dispatch(children);
           return TaskResult.Success;
        }

        private TaskResult HandleSpecTask(SpecTask specTask, List<TaskExecutionNode> children)
        {
            return TaskResult.Success;
        }

        private TaskResult HandleExecuteElementTask(ExecuteElementTask executeElementTask, List<TaskExecutionNode> children)
        {
            
            return TaskResult.Success;
        }

        private void SetHandler<T>(RemoteTaskHandler<T> action) where T : RemoteTask
        {
            _handlers.Add(typeof (T), (task, children) => action((T)task, children));
        }

        public override void ExecuteRecursive(TaskExecutionNode node)
        {
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