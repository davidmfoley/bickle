using System;
using System.Xml;
using JetBrains.ReSharper.TaskRunnerFramework;

namespace Bickle.ReSharper.Runner.Tasks
{
    [Serializable]
    public class LoadContextAssemblyTask : RemoteTask
    {
        public readonly string AssemblyPath;

        public LoadContextAssemblyTask(XmlElement element)
            : base(element)
        {
            AssemblyPath = GetXmlAttribute(element, "AssemblyPath");
        }

        public LoadContextAssemblyTask(string assemblyPath)
            : base(BickleTaskRunner.RunnerId)
        {
            AssemblyPath = assemblyPath;
        }

        public override void SaveXml(System.Xml.XmlElement element)
        {
            base.SaveXml(element);
            SetXmlAttribute(element, "AssemblyPath", AssemblyPath);

        }

        public bool Equals(LoadContextAssemblyTask other)
        {
            return other != null && other.AssemblyPath == AssemblyPath;
        }

        public override bool Equals(RemoteTask other)
        {
            if (other == null) return false;

            return ReferenceEquals(this, other) ||
                   Equals(other as LoadContextAssemblyTask);
        }
    }
}