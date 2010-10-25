using NUnit.Framework;

namespace Bickle.Tests
{
    [TestFixture]
    public class Executing_examples
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUpContext()
        {
            Listener = new FakeResultListener();
        }

        #endregion

        private FakeResultListener Listener;

        [Test]
        public void handles_ignored_tests()
        {
            var container = new InactiveExampleContainer("Foo", null);
            container.AddIt(new Example("Bar", () => Assert.IsTrue(false), container));

            container.Execute(Listener);

            Listener.Calls[0].ShouldBe("Ignored - Foo\r\nBar");
        }

        [Test]
        public void handles_predicate_expression_failure()
        {
            int foo = 4;
            var it = new Example("Bar", () => foo == 5, new ActiveExampleContainer("Foo", null));

            it.Execute(Listener);

            Listener.Calls[0].ShouldStartWith("Failed - Foo\r\nBar");
        }

        [Test]
        public void handles_predicate_expression_success()
        {
            var it = new Example("Bar", () => true, new ActiveExampleContainer("Foo", null));

            it.Execute(Listener);

            Listener.Calls[0].ShouldBe("Success - Foo\r\nBar");
        }

        [Test]
        public void notifies_listener_on_failure()
        {
            var it = new Example("Bar", () => Assert.IsTrue(false), new ActiveExampleContainer("Foo", null));

            it.Execute(Listener);

            Listener.Calls[0].ShouldBe("Failed - Foo\r\nBar - AssertionException");
        }

        [Test]
        public void notifies_listener_on_success()
        {
            var it = new Example("Bar", () => Assert.IsTrue(true), new ActiveExampleContainer("Foo", null));

            it.Execute(Listener);

            Listener.Calls[0].ShouldBe("Success - Foo\r\nBar");
        }

        [Test]
        public void pending_example()
        {
            var it = new Example("Bar", Spec.Pending, new ActiveExampleContainer("Foo", null));

            it.Execute(Listener);

            Listener.Calls[0].ShouldStartWith("Pending - Foo\r\nBar");
        }
    }
}