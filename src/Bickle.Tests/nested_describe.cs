﻿using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace Bickle.Tests
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
            Describes[0].Describes[0].Examples.Length.ShouldBe(1);
            Describes[0].Describes[0].Examples[0].Action();
            NestedDescribe.ItWasCalled.ShouldBe(true);
        }

        [Test]
        public void Should_execute_in_correct_order()
        {
            var spec = new NestedDescribe();
            spec.Execute(new FakeResultListener());
            var expected = new[] { "OuterBefore", "InnerBefore", "It", "InnerAfter", "OuterAfter" };
            Assert.That(spec.Events, Is.EquivalentTo(expected));
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
                    It("Should Baz", () => { 
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