using System;
using System.Linq.Expressions;

namespace Bickle
{
    public abstract class ExampleNode : IExampleNode
    {
        public ExampleContainer Parent { get; set; }
        public abstract void Execute(ITestResultListener listener);
        public string Name { get; set; }
        public abstract bool IsIgnored();

        public ISpec ContainingSpec { get; set; }

        protected ExampleNode(ExampleContainer parent, string name, Spec spec)
        {
            Parent = parent;
            Name = name;
            ContainingSpec = spec;
        }

        public string FullName
        {
            get { return (Parent != null ? Parent.FullName + "\r\n" : "") + Name; }
        }

        public string Id { get; set; }
        
    }
}