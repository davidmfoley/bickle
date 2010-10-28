using System;
using System.Linq.Expressions;

namespace Bickle
{
    public class ExampleNode
    {
        public ExampleContainer Parent { get; set; }
        public string Name { get; set; }
        public Spec Spec { get; set; }

        protected ExampleNode(ExampleContainer parent, string name, Spec spec)
        {
            Parent = parent;
            Name = name;
            Spec = spec;
        }

        public string FullName
        {
            get { return (Parent != null ? Parent.FullName + "\r\n" : "") + Name; }
        }

        public string Id { get; set; }
    }
}