using System.Xml;
using Bickle.ReSharper.Runner;
using JetBrains.ReSharper.TaskRunnerFramework;

namespace Bickle.ReSharper.Tasks
{
    public class ElementContainerTask : RemoteTask
    {
         public string Id;

        public ElementContainerTask(string id) : base(BickleTaskRunner.RunnerId)
        {
            Id = id;
        }

        public ElementContainerTask(XmlElement element)
            : base(element)
        {
            Id = GetXmlAttribute(element, "Id");
        }

        public override bool Equals(RemoteTask other)
        {
            if (other is ElementContainerTask)
                return ((ElementContainerTask)other).Id == Id;

            return false;
        }

        public override void SaveXml(XmlElement element)
        {
            SetXmlAttribute(element, "Id", Id);
            base.SaveXml(element);
        }
    }
}