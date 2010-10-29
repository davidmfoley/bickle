using System.Linq;

namespace Bickle
{
    public class ExampleContainerWrapper : IExampleContainer
    {
        private readonly object _inner;

        public ExampleContainerWrapper(object inner)
        {
            _inner = inner;
        }

        public IExampleContainer[] ExampleContainers
        {
            get
            {
                return
                    ((object[])_inner
                                   .GetPropertyWithReflection("ExampleContainers"))
                        .Select(c => new ExampleContainerWrapper(c))
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
                        .Select(c => new ExampleWrapper(c))
                        .ToArray();
            }
        }

        public string Id
        {
            get { return (string)_inner.GetPropertyWithReflection("Id"); }
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