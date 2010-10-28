using System;
using System.Xml;
using Bickle.ReSharper.Runner;
using JetBrains.ReSharper.TaskRunnerFramework;

namespace Bickle.ReSharper.Tasks
{
    [Serializable]
    public class ExecuteElementTask : RemoteTask
    {
        public string Id;

        public ExecuteElementTask(string id) : base(BickleTaskRunner.RunnerId)
        {
            Id = id;
        }

        public ExecuteElementTask(XmlElement element) : base(element)
        {
            Id = GetXmlAttribute(element, "Id");
        }

        public override bool Equals(RemoteTask other)
        {
            if (other is ExecuteElementTask)
                return ((ExecuteElementTask) other).Id == Id;

            return false;
        }

        public override void SaveXml(XmlElement element)
        {
            SetXmlAttribute(element, "Id", Id);
            base.SaveXml(element);
        }
    }
}