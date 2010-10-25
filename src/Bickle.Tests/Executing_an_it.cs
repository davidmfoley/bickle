using NUnit.Framework;

namespace Bickle.Tests
{
    [TestFixture]
    public class Executing_an_it
    {
        private FakeResultListener Listener;

        [SetUp]
        public void SetUpContext()
        {
            Listener = new FakeResultListener();
        }

        [Test]
        public void notifies_listener_on_success()
        {
            var it = new Example("Bar", () => Assert.IsTrue(true), new ExampleContainer("Foo", null));

            it.Execute(Listener);

            Listener.Calls[0].ShouldBe("Success - Foo, Bar");
        }

        [Test]
        public void notifies_listener_on_failure()
        {
            var it = new Example("Bar", () => Assert.IsTrue(false), new ExampleContainer("Foo", null));

            it.Execute(Listener);

            Listener.Calls[0].ShouldBe("Failed - Foo, Bar - AssertionException");
        }

        [Test]
        public void handles_predicate_expression_failure()
        {
            int foo = 4;
            var it = new Example("Bar", () => foo == 5, new ExampleContainer("Foo", null));

            it.Execute(Listener);

            Listener.Calls[0].ShouldStartWith("Failed - Foo, Bar");
        }

        [Test]
        public void handles_predicate_expression_success()
        {
            var it = new Example("Bar", () => true, new ExampleContainer("Foo", null));

            it.Execute(Listener);

            Listener.Calls[0].ShouldBe("Success - Foo, Bar");
        }


    }

    [TestFixture]
    public class Describing_a_spec
    {
        [Test]
        public void Can_describe_an_equals()
        {
            int foo = 4;
            var description = SpecDescriber.DescribeSpec(() => foo == 5);
            description.ShouldBe("foo == 5");
        }

        [Test]
        public void Can_describe_not_equals()
        {
            int foo = 4;
            var description = SpecDescriber.DescribeSpec(() => foo != 5);
            description.ShouldBe("foo != 5");
        }

        [Test]
        public void Can_describe_greater_than()
        {
            int foo = 4;
            int bar = 5;
            var description = SpecDescriber.DescribeSpec(() => foo > bar);
            description.ShouldBe("foo > bar");
        }
    }
}