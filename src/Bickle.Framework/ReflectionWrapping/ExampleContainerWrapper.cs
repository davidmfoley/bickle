using System;
using System.Linq;
using Bickle.Utility;

namespace Bickle.ReflectionWrapping
{
    public class ExampleContainerWrapper : IExampleContainer
    {
        private readonly object _inner;

        public ExampleContainerWrapper(object inner, ISpec spec)
        {
            _inner = inner;
            ContainingSpec = spec;
        }

        public IExampleContainer[] ExampleContainers
        {
            get
            {
                return
                    ((object[])_inner
                                   .GetPropertyWithReflection("ExampleContainers"))
                        .Select(c => new ExampleContainerWrapper(c, ContainingSpec))
                        .ToArray();
            }
        }

        public IExample[] Examples
        {
            get
            {
                return
                    ((object[])_inner
                                   .GetPropertyWithReflection("Examples"))
                        .Select(c => new ExampleWrapper(c, this.ContainingSpec))
                        .ToArray();
            }
        }

        public ISpec ContainingSpec { get; private set; }

        public string Id
        {
            get { return (string)_inner.GetPropertyWithReflection("Id"); }
        }

        public void Execute(ITestResultListener listener)
        {
             _inner.InvokeWithReflection("Execute", ListenerWrapper.GetWrapperForTargetType(_inner.GetType(), listener, ContainingSpec));
        }

        public string Name
        {
            get { return (string)_inner.GetPropertyWithReflection("Name"); }
        }

        public bool IsIgnored()
        {
            return (bool)_inner.InvokeWithReflection("IsIgnored");
        }
    }
}