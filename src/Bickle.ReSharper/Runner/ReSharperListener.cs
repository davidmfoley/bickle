using System;
using JetBrains.ReSharper.TaskRunnerFramework;

namespace Bickle.ReSharper.Runner
{
    public class ReSharperListener : ITestResultListener
    {
        private IRemoteTaskServer _server;

        public ReSharperListener(IRemoteTaskServer server)
        {
            _server = server;
        }

        public void Failed(Example example, Exception exception)
        {
            _server.TaskFinished(CurrentTask, exception.ToString(), TaskResult.Exception);
        }

        public RemoteTask CurrentTask { get; set; }

        public void Success(Example example)
        {
            _server.TaskFinished(CurrentTask, "" , TaskResult.Success);  
        }

        public void Finished()
        {
                
        }

        public void Pending(Example example)
        {
            _server.TaskFinished(CurrentTask, "Pending", TaskResult.Skipped);  
        }

        public void Ignored(Example example)
        {
            _server.TaskFinished(CurrentTask, "Ignored", TaskResult.Skipped);  
        }
    }
}