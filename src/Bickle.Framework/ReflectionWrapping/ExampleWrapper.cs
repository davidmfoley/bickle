using System;
using Bickle.Utility;

namespace Bickle.ReflectionWrapping
{
    public class ExampleWrapper :IExample
    {
        private object _inner;
        private readonly ISpec _spec;

        public ExampleWrapper(object o, ISpec spec)
        {
            _inner = o;
            _spec = spec;
        }

        public string Id
        {
            get { return (string)_inner.GetPropertyWithReflection("Id"); }
        }

        public void Action()
        {
            _inner.InvokeWithReflection("Action");
        }

        public string FullName
        {
            get { return (string)_inner.GetPropertyWithReflection("FullName"); }
        }

        public void Execute(ITestResultListener listener)
        {

        }

        public string Name
        {
            get { return (string)_inner.GetPropertyWithReflection("Name"); }
        }

        public bool IsIgnored()
        {
            return (bool)_inner.InvokeWithReflection("IsIgnored");
        }

        public ISpec ContainingSpec
        {
            get { return _spec; }
        }
    }
}