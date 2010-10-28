using System;
using System.Collections.Generic;
using System.Linq;

namespace Bickle
{
    public abstract class ExampleContainer : ExampleNode
    {        
        private readonly List<ExampleContainer> _describes = new List<ExampleContainer>();
        private readonly List<Example> _its = new List<Example>();

        public Action After = () => { };
        public Action Before = () => { };

        protected ExampleContainer(string name, ExampleContainer parent, Spec spec) : base(parent, name, spec)
        {
            Spec = spec;
        }

        public Example[] Examples
        {
            get { return _its.ToArray(); }
        }

        public ExampleContainer[] ExampleContainers
        {
            get { return _describes.ToArray(); }
        }

        public Spec Spec;
   
        public abstract void Execute(ITestResultListener listener);

        public void AddIt(Example example)
        {
            example.Id = this.Id + "/" + _its.Count.ToString("000");
            _its.Add(example);
        }

        public void AddDescribe(ExampleContainer exampleContainer)
        {
            _describes.Add(exampleContainer);
        }

        protected IEnumerable<Action> GetBefores()
        {
            if (Parent == null)
                return new[] {Before};

            return new[] {Before}.Union(Parent.GetBefores());
        }

        protected IEnumerable<Action> GetAfters()
        {
            if (Parent == null)
                return new[] {After};

            return (new[] {After}.Union(Parent.GetAfters())).Reverse();
        }
    }
}