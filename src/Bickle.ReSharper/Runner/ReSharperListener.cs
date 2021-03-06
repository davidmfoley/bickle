﻿using System;
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

        public void Failed(IExample example, Exception exception)
        {
            LastResult = TaskResult.Exception;
            _server.TaskOutput(CurrentTask, "Failed:", TaskOutputType.STDERR);
            _server.TaskOutput(CurrentTask, exception.ToString(), TaskOutputType.STDERR);
            _server.TaskFinished(CurrentTask, exception.ToString(), TaskResult.Exception);
        }

        public RemoteTask CurrentTask { get; set; }

        public TaskResult LastResult { get; set; }

        public void Success(IExample example)
        {
            LastResult = TaskResult.Success;
            _server.TaskFinished(CurrentTask, "" , TaskResult.Success);  
        }

        public void Finished()
        {
                
        }

        public void Pending(IExample example)
        {
            LastResult = TaskResult.Skipped;
            _server.TaskFinished(CurrentTask, "Pending", TaskResult.Skipped);  
        }

        public void Ignored(IExample example)
        {
            LastResult = TaskResult.Skipped;
            _server.TaskFinished(CurrentTask, "Ignored", TaskResult.Skipped);  
        }
    }
}