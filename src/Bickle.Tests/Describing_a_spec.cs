using NUnit.Framework;

namespace Bickle.Tests
{
    [TestFixture]
    public class Describing_a_spec
    {
        [Test]
        public void Can_describe_an_equals()
        {
            int foo = 4;
            string description = SpecDescriber.DescribeSpec(() => foo == 5);
            description.ShouldBe("foo should equal 5");
        }

        [Test]
        public void Can_describe_greater_than()
        {
            int foo = 4;
            int bar = 5;
            string description = SpecDescriber.DescribeSpec(() => foo > bar);
            description.ShouldBe("foo should be greater than bar");
        }

        [Test]
        public void Can_describe_not_equals()
        {
            int foo = 4;
            string description = SpecDescriber.DescribeSpec(() => foo != 5);
            description.ShouldBe("foo should not equal 5");
        }
    }
}