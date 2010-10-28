using System.Drawing;
using Bickle.ReSharper.Provider.Elements;
using JetBrains.CommonControls;
using JetBrains.ReSharper.UnitTestFramework;
using JetBrains.ReSharper.UnitTestFramework.UI;
using JetBrains.TreeModels;
using JetBrains.UI.TreeView;

namespace Bickle.ReSharper
{
    internal class BickleElementPresenter
    {
        public void Present(UnitTestElement element, IPresentableItem item, TreeModelNode node, PresentationState state)
        {
            item.RichText = element.GetTitle();

            var standardImage = GetImage(element);
            var stateImage = UnitTestManager.GetStateImage(state);
            if (stateImage != null)
            {
                item.Images.Add(stateImage);
            }
            else if (standardImage != null)
            {
                item.Images.Add(standardImage);
            }
        }

        private Image GetImage(UnitTestElement element)
        {
            var elementImage = element is ExampleElement ? UnitTestElementImage.Test : UnitTestElementImage.TestContainer;
            return UnitTestManager.GetStandardImage(elementImage);
        }
    }
}