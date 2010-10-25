using System;
using System.Collections.Generic;
using Bickle;
using NUnit.Framework;

namespace Bickle.Tests
{
    public class SingleDescribe : Spec
    {
        public static bool BeforeWasCalled;
        public static bool AfterWasCalled;
        public static bool ItWasCalled;

        public SingleDescribe()
        {
            Describe("Foo", () =>
            {
                Before(() => BeforeWasCalled = true);
                It("should bar", () => ItWasCalled = true);
                After(() => AfterWasCalled = true);
            });
        }
    }

    [TestFixture]
    public class single_describe : SpecFor<SingleDescribe>
    {      
        [Test]
        public void has_a_single_describe()
        {           
            Describes.Length.ShouldBe(1);
            Describes[0].Name.ShouldBe("Foo");
        }

        [Test]
        public void has_a_single_it()
        {
            Describes[0].Its.Length.ShouldBe(1);
            Describes[0].Its[0].Name.ShouldBe("should bar");
            SingleDescribe.ItWasCalled = false;
            Describes[0].Its[0].Action();
            SingleDescribe.ItWasCalled.ShouldBe(true);
        }

        [Test]
        public void has_a_before()
        {
            SingleDescribe.BeforeWasCalled = false;
            Describes[0].Before();
            SingleDescribe.BeforeWasCalled.ShouldBe(true);
        }

        [Test]
        public void has_an_after()
        {
            SingleDescribe.AfterWasCalled = false;
            Describes[0].After();
            SingleDescribe.AfterWasCalled.ShouldBe(true);
        }   
    }

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
            var it = new It("Bar", () => Assert.IsTrue(true), new Describe("Foo", null));

            it.Execute(Listener);

            Listener.Calls[0].ShouldBe("Success - Foo, Bar");
        }

        [Test]
        public void notifies_listener_on_failure()
        {
            var it = new It("Bar", () => Assert.IsTrue(false), new Describe("Foo", null));

            it.Execute(Listener);

            Listener.Calls[0].ShouldBe("Failed - Foo, Bar - AssertionException");
        }
    }

    public class FakeResultListener : ITestResultListener
    {
        public List<string> Calls = new List<string>();
        public void Failed(It it, Exception exception)
        {
            Calls.Add("Failed - " + it.FullName + " - " + exception.GetType().Name);
        }

        public void Success(It it)
        {
            Calls.Add("Success - " + it.FullName);
        }
    }

   
}
