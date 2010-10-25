using System;
using NUnit.Framework;

namespace Bickle.Examples
{
    public class ExampleSpec : Spec
    {
        private int Befores;

        public ExampleSpec()
        {
            Describe("Bickle", () =>
            {
                Describe("Creating Examples", () =>
                {
                    int Foo = 2;
                    Before(() => Befores++);

                    It("supports using an action with an assert", () => Assert.IsTrue(true));

                    It("translates exceptions into failures", () => { throw new ApplicationException("Failed!"); return;});
                    It("also supports a predicate (func returning a bool)", () => Foo == 2);

                    Describe("Simple assertions can use Specify()", () =>
                    {
                        Specify(() => 100 == 100);
                        Specify(() => 101 > 100);
                        Specify(() => 99 <= 100);

                        Describe("Failures are translated", () =>
                        {
                            Specify(() => Foo > 100);
                            Specify(() => Foo == 100);
                            Specify(()=>5 < 6);
                        });
                    });

                    It("Can handle pending steps", Pending);
                });
            });
        }
    }
}
