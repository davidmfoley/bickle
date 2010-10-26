﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mime;
using System.Text;
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
        private BickleAssemblyExplorer _assemblyExplorer;
        private BickleElementPresenter _presenter;
        private BickleElementComparer _comparer;
        private BickleTaskFactory _taskFactory;


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
            get { return "StorEvil runner"; }
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
            ReadLockCookie.Execute(() => { _assemblyExplorer.ExploreProject(project, consumer); });
        }

        public void ExploreSolution(ISolution solution, UnitTestElementConsumer consumer)
        {
        }
    }

    internal class BickleTaskFactory
    {
        public IList<UnitTestTask> GetTasks(UnitTestElement element, IList<UnitTestElement> explicitElements)
        {
            return new List<UnitTestTask>();
        }
    }

    internal class BickleElementComparer
    {
        public bool IsDeclaredElementOfKind(IDeclaredElement declaredElement, UnitTestElementKind elementKind)
        {
            return false;
        }

        public int CompareUnitTestElements(UnitTestElement unitTestElement, UnitTestElement unitTestElement1)
        {
            return 0;
        }
    }

    internal class BickleElementPresenter
    {
        public void Present(UnitTestElement element, IPresentableItem item, TreeModelNode node, PresentationState state)
        {
            
        }
    }

    internal class BickleAssemblyExplorer
    {
        public void ExploreProject(IProject project, UnitTestElementConsumer consumer)
        {
            throw new NotImplementedException();
        }
    }

    public class BickleTaskRunner
    {
        public static string RunnerId;
    }
}
