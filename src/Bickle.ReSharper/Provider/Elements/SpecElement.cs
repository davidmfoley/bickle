using JetBrains.ProjectModel;
using JetBrains.ReSharper.UnitTestFramework;

namespace Bickle.ReSharper.Provider.Elements
{
    public class SpecElement : BickleUnitTestElement
    {
        private readonly Spec _spec;
        private readonly IProject _project;
        public string AssemblyLocation;

        public SpecElement(IUnitTestProvider provider,Spec spec, IProject project) 
            : base(provider, spec, project, null)
        {
            _spec = spec;
            _project = project;
            var type = _spec.GetType();
            Id = type.FullName;
            AssemblyLocation = type.Assembly.Location;
        }

        public override string GetTitle()
        {
            return ShortName;
        }

        public override string ShortName
        {
            get { return _spec.GetType().Name; }
        }
    }
}