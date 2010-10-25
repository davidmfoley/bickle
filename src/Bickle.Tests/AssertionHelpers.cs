using NUnit.Framework;

namespace Bickle.Tests
{
    public static class AssertionHelpers
    {
        public static void ShouldBe(this object actual, object expected)
        {
            Assert.AreEqual(expected, actual);
        }

        public static void ShouldStartWith(this string actual, string expected)
        {
            Assert.That(actual.StartsWith(expected), "Failed!\r\nExpected:\r\n '" + actual+ "'\r\nto start with\r\n '" + expected+"'" );
        }
    }
}