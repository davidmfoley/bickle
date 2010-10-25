using System;
using System.Collections.Generic;
using System.Linq;

namespace Bickle
{
    public class ExampleContainer
    {
        private readonly ExampleContainer _parent;
        private List<Example> _its = new List<Example>();
        public string Name { get; set; }

        public Example[] Examples
        {
            get { return _its.ToArray(); }
        }

        public ExampleContainer[] ExampleContainers
        {
            get { return _describes.ToArray(); }
        }

        public Action Before = () => { };
        public Action After = () => { };
        private readonly List<ExampleContainer> _describes = new List<ExampleContainer>();
        
        public string FullName
        {
            get { return (_parent != null ? _parent.FullName + "\r\n" : "") + Name; }
        }

        public ExampleContainer(string name, ExampleContainer parent)
        {
            _parent = parent;
            Name = name;
        }

        public void AddIt(Example example)
        {
            _its.Add(example);
        }

        public void AddDescribe(ExampleContainer exampleContainer)
        {
            _describes.Add(exampleContainer);
        }

        public void Execute(ITestResultListener listener)
        {
            foreach (var it in Examples)
            {
                foreach (var before in GetBefores())
                {
                    before();
                }

                it.Execute(listener);

                foreach (var after in GetAfters())
                {
                    after();
                }
            }

            foreach (var describe in ExampleContainers)
            {
                describe.Execute(listener);
            }
        }

        private IEnumerable<Action> GetBefores()
        {
            if (_parent == null)
                return new[] { Before };

           return  new[] {Before}.Union(_parent.GetBefores());
        }

        private IEnumerable<Action> GetAfters()
        {
            if (_parent == null)
                return new[] {After};

            return (new[] { After }.Union(_parent.GetAfters())).Reverse();
        }
    }
}