using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace Bickle.Tests
{
    [TestFixture]
    public class nested_describe : SpecFor<NestedDescribe>
    {
        [Test]
        public void Should_execute_in_correct_order()
        {
            var spec = new NestedDescribe();
            spec.Execute(new FakeResultListener());
            var expected = new[] {"OuterBefore", "InnerBefore", "It", "InnerAfter", "OuterAfter"};
            Assert.That(spec.Events, Is.EquivalentTo(expected));
        }

        [Test]
        public void Should_have_nested_describe()
        {
            ExampleContainers[0].ExampleContainers.Length.ShouldBe(1);
            ExampleContainers[0].ExampleContainers[0].Name.ShouldBe("Bar");
        }

        [Test]
        public void Should_have_nested_it()
        {
            NestedDescribe.ItWasCalled = false;
            ExampleContainers[0].ExampleContainers[0].Examples.Length.ShouldBe(1);
            ExampleContainers[0].ExampleContainers[0].Examples[0].Action();
            NestedDescribe.ItWasCalled.ShouldBe(true);
        }

        [Test]
        public void Should_have_one_describe()
        {
            ExampleContainers.Length.ShouldBe(1);
            ExampleContainers[0].Name.ShouldBe("Foo");
        }
    }

    [TestFixture]
    public class ReflectionWrapping_to_handle_version_conflicts
    {
        private SpecWrapper Wrapper;

        [SetUp]
        public void SetUpContext()
        {
            var spec = new NestedDescribe();

            Wrapper = new SpecWrapper(spec);
        }
       

        [Test]
        public void Should_have_nested_describe()
        {
            ExampleContainers[0].ExampleContainers.Length.ShouldBe(1);
            ExampleContainers[0].ExampleContainers[0].Name.ShouldBe("Bar");
        }

        protected IExampleContainer[] ExampleContainers
        {
            get { return Wrapper.ExampleContainers; }
        }

        [Test]
        public void Should_have_nested_it()
        {
            NestedDescribe.ItWasCalled = false;
            ExampleContainers[0].ExampleContainers[0].Examples.Length.ShouldBe(1);
            ExampleContainers[0].ExampleContainers[0].Examples[0].Action();
            NestedDescribe.ItWasCalled.ShouldBe(true);
        }

        [Test]
        public void Should_have_one_describe()
        {
            ExampleContainers.Length.ShouldBe(1);
            ExampleContainers[0].Name.ShouldBe("Foo");
        }

        
    }


    public class NestedDescribe : Spec
    {
        public static bool ItWasCalled;
        public List<string> Events = new List<string>();

        public NestedDescribe()
        {
            Describe("Foo", () =>
            {
                Before(() => Record("OuterBefore"));

                Describe("Bar", () =>
                {
                    Before(() => Record("InnerBefore"));
                    It("Should Baz", () =>
                    {
                        Record("It");
                        ItWasCalled = true;
                    });
                    After(() => Record("InnerAfter"));
                });

                After(() => Record("OuterAfter"));
            });
        }

        private void Record(string s)
        {
            Events.Add(s);
        }
    }
}