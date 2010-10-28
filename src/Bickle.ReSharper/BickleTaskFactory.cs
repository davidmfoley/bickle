using System;
using System.Collections.Generic;
using System.Xml;
using JetBrains.ReSharper.TaskRunnerFramework;
using JetBrains.ReSharper.UnitTestFramework;

namespace Bickle.ReSharper
{
    internal class BickleTaskFactory
    {
        public IList<UnitTestTask> GetTasks(UnitTestElement element, IList<UnitTestElement> explicitElements)
        {
             
            var tasks = new List<UnitTestTask>();
            var example = element as ExampleElement;
            if (example != null)
                tasks.Add(new UnitTestTask(null, new  ExecuteElementTask(example.Id)));
            return tasks;
        }
    }

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