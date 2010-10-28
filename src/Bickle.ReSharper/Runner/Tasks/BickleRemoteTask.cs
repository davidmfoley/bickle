using System;
using System.Xml;
using JetBrains.ReSharper.TaskRunnerFramework;

namespace Bickle.ReSharper.Runner.Tasks
{
    [Serializable]
    public abstract class BickleRemoteTask : RemoteTask
    {
        public string Id;

        public bool Explicitly { get; set; }

        protected BickleRemoteTask(XmlElement element) : base(element)
        {
            Id = GetXmlAttribute(element, "Id");
        }

        protected BickleRemoteTask(string id, bool explicitly) : base(BickleTaskRunner.RunnerId)
        {
            Id = id;
        }
        
        public override void SaveXml(XmlElement element)
        {
            SetXmlAttribute(element, "Id", Id);
            base.SaveXml(element);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = base.GetHashCode();
                result = (result*397) ^ (Id != null ? Id.GetHashCode() : 0);
                result = (result*397) ^ Explicitly.GetHashCode();
                return result;
            }
        }
    }
}