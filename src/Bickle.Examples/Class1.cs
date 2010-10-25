using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Bickle.Examples
{
    public class ExampleSpec : Spec
    {
        private int _foo;

        public ExampleSpec()
        {
            Describe("An example spec", () =>
            {
                Describe("an inner spec", () =>
                {
                    Before(() => _foo++);

                    It("should have called before once", () => Assert.AreEqual(1, _foo));
                    It("should have called before again", () => _foo == 2);
                });
            });
        }
    }
}
