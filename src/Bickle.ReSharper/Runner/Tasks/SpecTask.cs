using System;
using System.Xml;
using JetBrains.ReSharper.TaskRunnerFramework;

namespace Bickle.ReSharper.Runner.Tasks
{
    [Serializable]
    public class SpecTask : BickleRemoteTask, IEquatable<SpecTask>
    {


        public SpecTask(string typeName, bool explicitly)
            : base(typeName, explicitly)
        {           
        }

        public SpecTask(XmlElement element)
            : base(element)
        {           
        }

        public override bool Equals(RemoteTask other)
        {
            if (other is SpecTask)
                return Equals((SpecTask) other);

            return false;
        }


        public bool Equals(SpecTask other)
        {
            return other.Id == Id && other.Explicitly == Explicitly;
        }
    }
}