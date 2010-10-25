using System;
using System.Collections.Generic;
using System.Linq;

namespace Bickle
{
    public abstract class ExampleContainer
    {
        private readonly List<ExampleContainer> _describes = new List<ExampleContainer>();
        private readonly List<Example> _its = new List<Example>();
        private readonly ExampleContainer _parent;
        public Action After = () => { };
        public Action Before = () => { };

        protected ExampleContainer(string name, ExampleContainer parent)
        {
            _parent = parent;
            Name = name;
        }

        public string Name { get; set; }

        public Example[] Examples
        {
            get { return _its.ToArray(); }
        }

        public ExampleContainer[] ExampleContainers
        {
            get { return _describes.ToArray(); }
        }

        public string FullName
        {
            get { return (_parent != null ? _parent.FullName + "\r\n" : "") + Name; }
        }

        public abstract void Execute(ITestResultListener listener);

        public void AddIt(Example example)
        {
            _its.Add(example);
        }

        public void AddDescribe(ExampleContainer exampleContainer)
        {
            _describes.Add(exampleContainer);
        }

        protected IEnumerable<Action> GetBefores()
        {
            if (_parent == null)
                return new[] {Before};

            return new[] {Before}.Union(_parent.GetBefores());
        }

        protected IEnumerable<Action> GetAfters()
        {
            if (_parent == null)
                return new[] {After};

            return (new[] {After}.Union(_parent.GetAfters())).Reverse();
        }
    }
}