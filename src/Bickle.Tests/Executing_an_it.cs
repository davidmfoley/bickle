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
            var it = new Example("Bar", () => Assert.IsTrue(true), new Describe("Foo", null));

            it.Execute(Listener);

            Listener.Calls[0].ShouldBe("Success - Foo, Bar");
        }

        [Test]
        public void notifies_listener_on_failure()
        {
            var it = new Example("Bar", () => Assert.IsTrue(false), new Describe("Foo", null));

            it.Execute(Listener);

            Listener.Calls[0].ShouldBe("Failed - Foo, Bar - AssertionException");
        }

        [Test]
        public void handles_predicate_expression()
        {
            var it = new Example("Bar", () => false, new Describe("Foo", null));

            it.Execute(Listener);

            Listener.Calls[0].ShouldStartWith("Failed - Foo, Bar");
        }


    }
}