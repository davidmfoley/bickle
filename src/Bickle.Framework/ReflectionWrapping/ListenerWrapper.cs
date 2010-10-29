using System;
using Bickle.Utility;

namespace Bickle.ReflectionWrapping
{
    public class ListenerWrapper : ITestResultListener
    {
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