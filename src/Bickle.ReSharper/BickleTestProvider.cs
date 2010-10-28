using System;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Application;
using JetBrains.CommonControls;
using JetBrains.Metadata.Reader.API;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.TaskRunnerFramework;
using JetBrains.ReSharper.UnitTestFramework;
using JetBrains.ReSharper.UnitTestFramework.UI;
using JetBrains.TreeModels;
using JetBrains.UI.TreeView;

namespace Bickle.ReSharper
{
    [UnitTestProvider]
    public class BickleTestProvider : IUnitTestProvider
    {
        private readonly BickleAssemblyExplorer _assemblyExplorer;
        private readonly BickleElementPresenter _presenter = new BickleElementPresenter();
        private readonly BickleElementComparer _comparer =  new BickleElementComparer();
        private readonly BickleTaskFactory _taskFactory = new BickleTaskFactory();


        public BickleTestProvider()
        {
            _assemblyExplorer = new BickleAssemblyExplorer(this);
        }

        public ProviderCustomOptionsControl GetCustomOptionsControl(ISolution solution)
        {
            return null;
        }

        public RemoteTaskRunnerInfo GetTaskRunnerInfo()
        {
            var runnerInfo = new RemoteTaskRunnerInfo(typeof(BickleTaskRunner));
            return runnerInfo;
        }

        public string Serialize(UnitTestElement element)
        {
            return "";
        }

        public IList<UnitTestTask> GetTaskSequence(UnitTestElement element, IList<UnitTestElement> explicitElements)
        {
            return _taskFactory.GetTasks(element, explicitElements);
        }

        public int CompareUnitTestElements(UnitTestElement x, UnitTestElement y)
        {
            return _comparer.CompareUnitTestElements(x, y);
        }

        public string ID
        {
            get { return BickleTaskRunner.RunnerId; }
        }

        public string Name
        {
            get { return "Bickle runner"; }
        }

        public System.Drawing.Image Icon
        {
            get { return null; }
        }


        public UnitTestElement Deserialize(ISolution solution, string elementString)
        {
            throw new NotImplementedException();
        }

        public bool IsElementOfKind(UnitTestElement element, UnitTestElementKind elementKind)
        {
            return false;
        }

        public void Present(UnitTestElement element, IPresentableItem item, TreeModelNode node, PresentationState state)
        {
            _presenter.Present(element, item, node, state);
        }

        public bool IsElementOfKind(IDeclaredElement declaredElement, UnitTestElementKind elementKind)
        {
            return _comparer.IsDeclaredElementOfKind(declaredElement, elementKind);
        }

        public void ExploreFile(IFile psiFile, UnitTestElementLocationConsumer consumer, CheckForInterrupt interrupted)
        {
         
        }

        public void ExploreExternal(UnitTestElementConsumer consumer)
        {
            Debug.WriteLine("ExploreExternal");
        }

        public void ExploreAssembly(IMetadataAssembly assembly, IProject project, UnitTestElementConsumer consumer)
        {
            ReadLockCookie.Execute(() => { _assemblyExplorer.ExploreAssembly(assembly, project, consumer); });
        }

        public void ExploreSolution(ISolution solution, UnitTestElementConsumer consumer)
        {
        }
    }
}
