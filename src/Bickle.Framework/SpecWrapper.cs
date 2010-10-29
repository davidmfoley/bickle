using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Bickle;

namespace Bickle
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
            return new ExampleContainerWrapper(o);
        }

        public string Name
        {
            get { return (string)_inner.InvokeWithReflection("Name"); }
        }

        public bool IsIgnored()
        {
            return (bool)_inner.InvokeWithReflection("IsIgnored");
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
            var wrappedListenerType = GetListenerWrapperType();

            var wrappedListener = Activator.CreateInstance(wrappedListenerType, new object[] { listener, new ExampleTranslator(this)});
            _inner.InvokeWithReflection("Execute", wrappedListener);
        }

        private Type GetListenerWrapperType()
        {
            var assembly = _inner.GetType().Assembly;
            foreach (var source in GetAssemblies())
            {
                var wrappedListenerType = source.GetType(typeof(ListenerWrapper).FullName);
                if (wrappedListenerType != null)
                    return wrappedListenerType;
            }

            return null;

        }

        private IEnumerable<Assembly> GetAssemblies()
        {
            var assembly = _inner.GetType().Assembly;
            yield return  assembly;
            foreach (var referencedAssembly in assembly.GetReferencedAssemblies())
            {
                yield return Assembly.Load(referencedAssembly);
            }
               
        }

        private object ExampleTranslate(object ex)
        {
            var id = (string) ex.GetPropertyWithReflection("Id");
            return Find(id);
        }

        public IExampleNode Find(string id)
        {
            return _idMap[id];
        }
    }

}