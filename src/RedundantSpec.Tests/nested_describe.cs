using NUnit.Framework;

namespace RedundantSpec.Tests
{
    [TestFixture]
    public class nested_describe : SpecFor<NestedDescribe>
    {
        [Test]
        public void Should_have_one_describe()
        {
            Describes.Length.ShouldBe(1);
            Describes[0].Name.ShouldBe("Foo");
        }

        [Test]
        public void Should_have_nested_describe()
        {
            Describes[0].Describes.Length.ShouldBe(1);
            Describes[0].Describes[0].Name.ShouldBe("Bar");
        }


        [Test]
        public void Should_have_nested_it()
        {
            NestedDescribe.ItWasCalled = false;
            Describes[0].Describes[0].Its.Length.ShouldBe(1);
            Describes[0].Describes[0].Its[0].Action();
            NestedDescribe.ItWasCalled.ShouldBe(true);
        }
    }

    public class NestedDescribe : Spec
    {
        public static bool ItWasCalled;

        public NestedDescribe()
        {
            Describe("Foo", ()=> {
                Describe("Bar", ()=> 
                    It("Should Baz", () => { ItWasCalled = true; })
                );
            });
        }
    }
}