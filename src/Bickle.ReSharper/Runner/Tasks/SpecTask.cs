using System.Xml;
using Bickle.ReSharper.Runner;
using JetBrains.ReSharper.TaskRunnerFramework;

namespace Bickle.ReSharper.Tasks
{
    public class SpecTask : RemoteTask
    {
        public string TypeName;

        public SpecTask(string typeName) : base(BickleTaskRunner.RunnerId)
        {
            TypeName = typeName;
        }

        public SpecTask(XmlElement element)
            : base(element)
        {
            TypeName = GetXmlAttribute(element, "TypeName");
        }

        public override bool Equals(RemoteTask other)
        {
            if (other is SpecTask)
                return ((SpecTask)other).TypeName == TypeName;

            return false;
        }

        public override void SaveXml(XmlElement element)
        {
            SetXmlAttribute(element, "TypeName", TypeName);
            base.SaveXml(element);
        }
    }
}