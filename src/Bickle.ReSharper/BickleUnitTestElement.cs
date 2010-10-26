using System.Collections.Generic;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.UnitTestFramework;

namespace Bickle.ReSharper
{
    public abstract class BickleUnitTestElement : UnitTestElement
    {
        private readonly Spec _spec;
        private readonly IProject _project;

        public BickleUnitTestElement(IUnitTestProvider provider,Spec spec, IProject project, UnitTestElement parent) : base(provider, parent)
        {
            _spec = spec;
            _project = project;
        }

        public override IList<IProjectFile> GetProjectFiles()
        {
            return new List<IProjectFile>();
        }

        public override UnitTestElementDisposition GetDisposition()
        {

            return new UnitTestElementDisposition(new UnitTestElementLocation[0], this);
        }

        public override IDeclaredElement GetDeclaredElement()
        {
            return null;
        }

        public override IProject GetProject()
        {
            return _project;
        }

        public override string GetTitle()
        {
            return _spec.GetType().FullName;
        }

        public override string GetTypeClrName()
        {
            return _spec.GetType().FullName;
        }

        public override UnitTestNamespace GetNamespace()
        {
            return new UnitTestNamespace(_spec.GetType().Namespace);
        }

        public override string GetKind()
        {
            return "Bickle";
        }
    }
}