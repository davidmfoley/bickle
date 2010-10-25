using NUnit.Framework;

namespace RedundantSpec.Tests
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
}
