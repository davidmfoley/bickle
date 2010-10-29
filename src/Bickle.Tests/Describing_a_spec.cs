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

        [Test]
        public void Can_describe_a_field_reference()
        {
            var Foo = new {Bar = 6};

            string description = SpecDescriber.DescribeSpec(() => Foo.Bar == 42);
            description.ShouldBe("Foo.Bar should equal 42");
        }

        [Test]
        public void Can_describe_a_function_invocation()
        {
            var foo = new ExampleFoo();

            string description = SpecDescriber.DescribeSpec(() => foo.Bar() == 42);
            description.ShouldBe("foo.Bar() should equal 42");
        }

        class ExampleFoo
        {
            public int Bar()
            {
                return 42;
            }
        }
    }
}