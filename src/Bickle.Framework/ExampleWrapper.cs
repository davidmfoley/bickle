using System;

namespace Bickle
{
    public class ExampleWrapper : IExample
    {
        private object _inner;

        public ExampleWrapper(object o)
        {
            _inner = o;
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
    }
}