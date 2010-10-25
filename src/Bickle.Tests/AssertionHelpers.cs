using NUnit.Framework;

namespace RedundantSpec.Tests
{
    public static class AssertionHelpers
    {
        public static void ShouldBe(this object actual, object expected)
        {
            Assert.AreEqual(expected, actual);
        }
    }
}