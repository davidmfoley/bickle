using System;
using System.Collections.Generic;
using System.Reflection;
using Bickle.Utility;

namespace Bickle.ReflectionWrapping
{
    public class ListenerWrapper : ITestResultListener
    {

        public static object GetWrapperForTargetType(Type type, ITestResultListener listener, ISpec spec)
        {           
            foreach (var source in GetAssemblies(type))
            {
                var wrappedListenerType = source.GetType(typeof(ListenerWrapper).FullName);
                if (wrappedListenerType != null)
                    return Activator.CreateInstance(wrappedListenerType, listener, new ExampleTranslator(spec));
            }

            return null;
        }

        private static IEnumerable<Assembly> GetAssemblies(Type t)
        {
            var assembly = t.Assembly;
            yield return assembly;
            foreach (var referencedAssembly in assembly.GetReferencedAssemblies())
            {
                yield return Assembly.Load(referencedAssembly);
            }

        }


        private readonly object _listener;
        private readonly object _exampleTranslator;

        public ListenerWrapper(object listener, object exampleTranslator)
        {
            _listener = listener;
            _exampleTranslator = exampleTranslator;
        }

        public void Failed(IExample example, Exception exception)
        {
            _listener.InvokeWithReflection("Failed", Translate(example), exception);
        }

        private object Translate(IExample example)
        {
            return _exampleTranslator.InvokeWithReflection("Translate", example);
        }

        public void Success(IExample example)
        {
            _listener.InvokeWithReflection("Success", Translate(example));
        }

        public void Finished()
        {
            _listener.InvokeWithReflection("Finished");
        }

        public void Pending(IExample example)
        {
            _listener.InvokeWithReflection("Pending", Translate(example));
        }

        public void Ignored(IExample example)
        {
            _listener.InvokeWithReflection("Pending", Translate(example));
        }
    }
}