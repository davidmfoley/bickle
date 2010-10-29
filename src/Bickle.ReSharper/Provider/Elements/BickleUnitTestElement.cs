using System;
using System.Collections.Generic;
using JetBrains.Application;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Caches;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Util;
using JetBrains.ReSharper.UnitTestFramework;

namespace Bickle.ReSharper.Provider.Elements
{
    public abstract class BickleUnitTestElement : UnitTestElement
    {
        private readonly ISpec _spec;
        private readonly IProject _project;
        public string Id;

        public BickleUnitTestElement(IUnitTestProvider provider, ISpec spec, IProject project, UnitTestElement parent) : base(provider, parent)
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
            IDeclaredElement declaredElement = this.GetDeclaredElement();
            if ((declaredElement == null) || !declaredElement.IsValid())
            {
                return UnitTestElementDisposition.InvalidDisposition;
            }
            var locations = new List<UnitTestElementLocation>();
            foreach (IDeclaration declaration in declaredElement.GetDeclarations())
            {
                var  containingFile = declaration.GetContainingFile();
                if (containingFile != null)
                {
                    locations.Add(new UnitTestElementLocation(containingFile.ProjectFile, declaration.GetNameDocumentRange().TextRange, declaration.GetDocumentRange().TextRange));
                }
            }
            return new UnitTestElementDisposition(locations, this);

        }

        public override IDeclaredElement GetDeclaredElement()
        {
            ITypeElement declaredType = GetDeclaredType();
            if (declaredType != null)
            {
                var ctor = declaredType.Constructors[0];
                return ctor;
            }
            return null;

        }

        private ITypeElement GetDeclaredType()
        {
            IProject module = GetProject();
            if (module == null)
            {
                return null;
            }
            ISolution solution = module.GetSolution();
            if (solution == null)
            {
                return null;
            }
            PsiManager instance = PsiManager.GetInstance(solution);
            using (ReadLockCookie.Create())
            {
                return instance.GetDeclarationsCache(DeclarationsScopeFactory.ModuleScope(PsiModuleManager.GetInstance(solution).GetPrimaryPsiModule(module), true), true).GetTypeElementByCLRName(_spec.GetType().FullName);
            }

        }

        public override IProject GetProject()
        {
            return _project;
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

        public override bool Equals(object obj)
        {
            if (obj is BickleUnitTestElement)
                return ((BickleUnitTestElement) obj).Id == Id;

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}