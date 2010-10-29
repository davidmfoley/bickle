using System;
using System.Collections.Generic;
using System.Linq;

namespace Bickle
{
    public abstract class ExampleContainer : ExampleNode, IExampleContainer
    {
        private readonly List<ExampleContainer> _describes = new List<ExampleContainer>();
        private readonly List<Example> _its = new List<Example>();

        public Action After = () => { };
        public Action Before = () => { };

        protected ExampleContainer(string name, ExampleContainer parent, Spec spec)
            : base(parent, name, spec)
        {
            ContainingSpec = spec;
        }

        public IExample[] Examples
        {
            get { return _its.Cast<IExample>().ToArray(); }
        }


        public IExampleContainer[] ExampleContainers
        {
            get { return _describes.Cast<IExampleContainer>().ToArray(); }
        }


        public void AddIt(Example example)
        {
            example.Id = Id + "/example" + _its.Count.ToString("000");
            _its.Add(example);
        }

        public void AddDescribe(ExampleContainer exampleContainer)
        {
            _describes.Add(exampleContainer);
        }

        protected IEnumerable<Action> GetBefores()
        {
            if (Parent == null)
                return new[] { Before };

            return new[] { Before }.Union(Parent.GetBefores());
        }

        protected IEnumerable<Action> GetAfters()
        {
            if (Parent == null)
                return new[] { After };

            return (new[] { After }.Union(Parent.GetAfters())).Reverse();
        }        
    }
}