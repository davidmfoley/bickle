using System;
using System.Collections.Generic;
using System.Reflection;
using Bickle.ReflectionWrapping;
using JetBrains.Metadata.Reader.API;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.UnitTestFramework;

namespace Bickle.ReSharper.Provider
{
    public class BickleAssemblyExplorer
    {
        private BickleTestProvider _provider;

        public BickleAssemblyExplorer(BickleTestProvider provider)
        {
            _provider = provider;
        }

        public void ExploreAssembly(IMetadataAssembly assembly, IProject project, UnitTestElementConsumer consumer)
        {
            var a = Assembly.LoadFrom(assembly.Location);
            var specTypes = FilterToSpecs(a.GetTypes());

            var elementFactory = new ElementFactory(project, consumer, _provider);

            foreach (var type in specTypes)
            {
                var instance = Activator.CreateInstance(type);
                
                var spec = (instance is ISpec) ?  (ISpec)instance : new SpecWrapper(instance);
               
                elementFactory.CreateContainerElements(spec);                
            }
           
        }

       
        private IEnumerable<Type> FilterToSpecs(Type[] getTypes)
        {
            foreach (var metadataTypeInfo in getTypes)
            {
                if (IsSpec(metadataTypeInfo))
                    yield return metadataTypeInfo;
            }
        }

        private bool IsSpec(Type t)
        {

            foreach (var i in t.GetInterfaces())
            {
                if (i.FullName == typeof(ISpec).FullName)
                    return true;
            }
            return false;
        }
    }
}