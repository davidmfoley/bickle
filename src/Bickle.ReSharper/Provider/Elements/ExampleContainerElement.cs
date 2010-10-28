using JetBrains.ProjectModel;
using JetBrains.ReSharper.UnitTestFramework;

namespace Bickle.ReSharper.Provider.Elements
{
    public class ExampleContainerElement : BickleUnitTestElement
    {
        private string _shortName;

        public ExampleContainerElement(IUnitTestProvider provider, IProject project, UnitTestElement parent, ExampleContainer container) 
            : base(provider, container.Spec, project, parent)
        {
            _shortName = container.Name;
            Id = container.Id;
        }

        public override string GetTitle()
        {
            return _shortName;
        }

        public override string ShortName
        {
            get { return _shortName; }
        }
    }
}