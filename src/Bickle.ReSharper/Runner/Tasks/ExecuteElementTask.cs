using System;
using System.Xml;
using JetBrains.ReSharper.TaskRunnerFramework;

namespace Bickle.ReSharper.Runner.Tasks
{
    [Serializable]
    public class ExecuteElementTask : BickleRemoteTask , IEquatable<ExecuteElementTask>
    {


        public ExecuteElementTask(string id, bool explicitly)
            : base(id, explicitly)
        {
           
        }

        public ExecuteElementTask(XmlElement element) : base(element)
        {
           
        }

        public override bool Equals(RemoteTask other)
        {
            if (other is ExecuteElementTask)
                return Equals((ExecuteElementTask) other);

            return false;
        }

        public bool Equals(ExecuteElementTask other)
        {
            return other.Id == Id;
        }
    }
}