using JetBrains.CommonControls;
using JetBrains.ReSharper.UnitTestFramework;
using JetBrains.TreeModels;
using JetBrains.UI.TreeView;

namespace Bickle.ReSharper
{
    internal class BickleElementPresenter
    {
        public void Present(UnitTestElement element, IPresentableItem item, TreeModelNode node, PresentationState state)
        {
            item.RichText = element.GetTitle();
        }
    }
}