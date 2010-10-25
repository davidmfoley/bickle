using NUnit.Framework;

namespace Bickle.Examples
{
    public class ExampleSpec : Spec
    {
        private int Befores;

        public ExampleSpec()
        {
            Describe("An example spec", () =>
            {
                Describe("an inner spec", () =>
                {
                    Before(() => Befores++);

                    It("should have called before once", () => Assert.AreEqual(1, Befores));
                    It("should have called before again", () => Befores == 2);
                    Specify(() => Befores > 100);
                    Specify(() => Befores < 100);
                });
            });
        }
    }
}
