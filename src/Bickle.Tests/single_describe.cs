using System;
using System.Collections.Generic;
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
                It("should bar", () => { ItWasCalled = true; });
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
            ExampleContainers.Length.ShouldBe(1);
            ExampleContainers[0].Name.ShouldBe("Foo");
        }

        [Test]
        public void has_a_single_it()
        {
            ExampleContainers[0].Examples.Length.ShouldBe(1);
            ExampleContainers[0].Examples[0].Name.ShouldBe("should bar");
            SingleDescribe.ItWasCalled = false;
            ExampleContainers[0].Examples[0].Action();
            SingleDescribe.ItWasCalled.ShouldBe(true);
        }

        [Test]
        public void has_a_before()
        {
            SingleDescribe.BeforeWasCalled = false;
            ExampleContainers[0].Before();
            SingleDescribe.BeforeWasCalled.ShouldBe(true);
        }

        [Test]
        public void has_an_after()
        {
            SingleDescribe.AfterWasCalled = false;
            ExampleContainers[0].After();
            SingleDescribe.AfterWasCalled.ShouldBe(true);
        }   
    }

    public class FakeResultListener : ITestResultListener
    {
        public List<string> Calls = new List<string>();
        public void Failed(Example example, Exception exception)
        {
            Calls.Add("Failed - " + example.FullName + " - " + exception.GetType().Name);
        }

        public void Success(Example example)
        {
            Calls.Add("Success - " + example.FullName);
        }
    }

   
}
