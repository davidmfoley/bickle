using JetBrains.ProjectModel;
using JetBrains.ReSharper.UnitTestFramework;

namespace Bickle.ReSharper.Provider.Elements
{
    internal class ExampleElement : BickleUnitTestElement
    {
        private string _shortName;

        public ExampleElement(BickleTestProvider provider, UnitTestElement parent, IProject project, Example example) : base(provider, example.Spec, project, parent)
        {
            _shortName = example.Name;
            Id = example.Id;
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