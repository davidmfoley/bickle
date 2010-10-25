using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Bickle;

namespace Bickle
{
    public class SpecRunner
    {
        public void Run(string assemblyLocation)
        {
            var listener = new ConsoleListener();

            foreach (var type in GetSpecTypes(assemblyLocation))
                ExecuteSpecs(type, listener);

            listener.Finished();
        }

        private void ExecuteSpecs(Type type, ConsoleListener listener)
        {
            var instance = (Spec)Activator.CreateInstance(type);

            instance.Execute(listener);

            var disposable = instance as IDisposable;
            if (disposable != null)
                disposable.Dispose();
        }

        private IEnumerable<Type> GetSpecTypes(string assemblyLocation)
        {
            var assembly = Assembly.LoadFrom(assemblyLocation);
            return assembly.GetTypes().Where(t => t.IsSubclassOf(typeof (Spec)));
        }
    }
}