using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.UnitTestFramework;

namespace Bickle.ReSharper
{
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
}