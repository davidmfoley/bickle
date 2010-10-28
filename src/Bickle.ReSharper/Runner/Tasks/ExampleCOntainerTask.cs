using System;
using System.Xml;
using JetBrains.ReSharper.TaskRunnerFramework;

namespace Bickle.ReSharper.Runner.Tasks
{
    [Serializable]
    public class ExampleContainerTask : BickleRemoteTask, IEquatable<ExampleContainerTask>
    {
    
        public ExampleContainerTask(string id, bool explicitly) : base(id, explicitly)
        {
          
        }

        public ExampleContainerTask(XmlElement element)
            : base(element)
        {
        }

        public override bool Equals(RemoteTask other)
        {
            if (other is ExampleContainerTask)
                return Equals((ExampleContainerTask) other);

            return false;
        }

        public bool Equals(ExampleContainerTask other)
        {
            return other.Id == Id;
        }
    }
}