using System;
using System.Linq;
using Bickle;

namespace Bickle
{
    public class SpecWrapper : ISpec
    {
        private object _inner;

        public SpecWrapper(Object spec)
        {
            _inner = spec;
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
    }

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
                    ((object[]) _inner
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

        public string Name
        {
            get { return (string) _inner.GetPropertyWithReflection("Name"); }
        }

        public bool IsIgnored()
        {
            return (bool) _inner.InvokeWithReflection("IsIgnored");
        }
    }

    public class ExampleWrapper : IExample
    {
        private object _inner;

        public ExampleWrapper(object o)
        {
            _inner = o;
        }

        public void Action()
        {
            _inner.InvokeWithReflection("Action");
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

    public static class ReflectionExtensions
    {
        public static object InvokeWithReflection(this object target, string method, params object[] parameters)
        {
            return target.GetType().GetMethod(method).Invoke(target, parameters);
        }

        public static object GetPropertyWithReflection(this object target, string prop)
        {
            return target.GetType().GetProperty(prop).GetValue(target, null);
        }
    }
}