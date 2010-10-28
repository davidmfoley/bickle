using System;
using System.Xml;
using JetBrains.ReSharper.TaskRunnerFramework;

namespace Bickle.ReSharper.Runner.Tasks
{
    [Serializable]
    public class SpecTask : BickleRemoteTask, IEquatable<SpecTask>
    {
        public string AssemblyLocation { get; set; }


        public SpecTask(XmlElement element)
            : base(element)
        {
            AssemblyLocation = GetXmlAttribute(element, "AssemblyLocation");
        }

        public SpecTask(string typeName, string assemblyLocation, bool explicitly) : base(typeName, explicitly)
        {
            AssemblyLocation = assemblyLocation;
        }

        public override bool Equals(RemoteTask other)
        {
            if (other is SpecTask)
                return Equals((SpecTask) other);

            return false;
        }

        public override void SaveXml(XmlElement element)
        {
            SetXmlAttribute(element, "AssemblyLocation", AssemblyLocation);
            base.SaveXml(element);
        }

        public bool Equals(SpecTask other)
        {
            return other.AssemblyLocation == AssemblyLocation && other.Id == Id && other.Explicitly == Explicitly;
        }
    }
}