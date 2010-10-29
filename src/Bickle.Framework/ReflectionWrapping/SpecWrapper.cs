using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Bickle.Utility;

namespace Bickle.ReflectionWrapping
{
    public class SpecWrapper : ISpec
    {
        private object _inner;
        private Dictionary<string, IExampleNode> _idMap = new Dictionary<string, IExampleNode>();

        public SpecWrapper(Object spec)
        {
            _inner = spec;
            BuildIdMap();
        }

        private void BuildIdMap()
        {
            foreach (var exampleContainer in ExampleContainers)
            {
                AddToIdMap(exampleContainer);
            }
        }

        private void AddToIdMap(IExampleContainer exampleContainer)
        {
            _idMap.Add(exampleContainer.Id, exampleContainer);
            foreach (var example in exampleContainer.Examples)
                _idMap.Add(example.Id, example);

            foreach (var child in exampleContainer.ExampleContainers)
                AddToIdMap(child);
        }

        private IExampleContainer WrapContainer(object o)
        {
            return new ExampleContainerWrapper(o, this);
        }

        public string Name
        {
            get { return (string)_inner.InvokeWithReflection("Name"); }
        }

        public bool IsIgnored()
        {
            return (bool)_inner.InvokeWithReflection("IsIgnored");
        }

        public ISpec ContainingSpec
        {
            get { return this; }
        }

        public IExampleContainer[] ExampleContainers
        {
            get
            {
                var containers = (object[]) _inner.GetPropertyWithReflection("ExampleContainers");
                return containers.Select(WrapContainer).ToArray();
            }
        }

        public string Id
        {
            get { return (string)_inner.GetPropertyWithReflection("Id"); }
        }

        public void Execute(ITestResultListener listener)
        {
            var wrappedListener = ListenerWrapper.GetWrapperForTargetType(_inner.GetType(),listener, this);
            _inner.InvokeWithReflection("Execute", wrappedListener);
        }

        public IExampleNode Find(string id)
        {
            return _idMap[id];
        }
    }

}