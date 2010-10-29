using Bickle.ReSharper.Provider.Elements;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.UnitTestFramework;

namespace Bickle.ReSharper
{
    public class ElementFactory
    {
        private readonly IProject _project;
        private readonly UnitTestElementConsumer _consumer;
        private readonly BickleTestProvider _provider;

        public ElementFactory(IProject project, UnitTestElementConsumer consumer, BickleTestProvider provider)
        {
            _project = project;
            _consumer = consumer;
            _provider = provider;
        }

        public void CreateContainerElements(ISpec spec)
        {
            var specElement = new SpecElement(_provider, spec, _project);
            _consumer(specElement);
            var exampleContainers = spec.ExampleContainers;
            CreateContainerElements(exampleContainers, specElement);
        }

        private void CreateContainerElements(IExampleContainer[] exampleContainers, UnitTestElement parent)
        {
            foreach (var exampleContainer in exampleContainers)
            {
                var element = new ExampleContainerElement(_provider, _project, parent, (ExampleContainer)exampleContainer);
                _consumer(element);
                
                CreateContainerElements(exampleContainer.ExampleContainers, element);

                foreach (var example in exampleContainer.Examples)
                    _consumer(new ExampleElement(_provider, element, _project, (Example)example));

            }
        }
    }
}