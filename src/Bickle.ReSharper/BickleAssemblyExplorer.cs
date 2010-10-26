using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Metadata.Reader.API;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.UnitTestFramework;

namespace Bickle.ReSharper
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
            var a = Assembly.Load(assembly.Location);
            var specTypes = FilterToSpecs(a.GetTypes());

            foreach (var type in specTypes)
            {
                var spec = (Spec)Activator.CreateInstance(type);
                consumer(new SpecElement(_provider, spec, project));
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
            return t.IsSubclassOf(typeof (Spec));
        }
    }
}