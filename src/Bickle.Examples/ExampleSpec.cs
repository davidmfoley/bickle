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
                    // you can declare context variables at any closure level you wish
                    int Foo = 2;
                    Before(() => Befores++);

                    It("supports using an action with an assert", () => Assert.IsTrue(true));

                    It("translates exceptions into failures", () =>
                    {
                        if (true) throw new ApplicationException("Failed!");
                        return;
                    });
                    It("also supports a predicate (func returning a bool)", () => Foo == 2);

                    Describe("Simple assertions can use Specify()", () =>
                    {
                        Specify(() => Foo == (99 + 5));
                        Specify(() => 101 > 100);
                        Specify(() => 99 <= 100);

                        Describe("Expect() is a synonym for Specify()", () =>
                        {
                            Expect(() => Foo == (99 + 5));
                            Expect(() => 101 > 100);
                            Expect(() => 99 <= 100);
                        });

                        Describe("Failures are translated into English", () =>
                        {
                            Specify(() => Foo > 100);
                            Specify(() => Foo == 100);
                            Specify(() => 5 < 6);
                        });
                    });


                    It("Can handle pending steps", Pending);

                    Describe("An Ignore block ignores its contents", () =>
                    {
                        Ignore("(optional) reason for ignoring goes here", () =>
                        {
                            Describe("Ignore can contain describes", () =>
                            {
                                It("will not run any contents of the ignore", () => false);
                                Expect(() => 0 == 1);
                            });

                            It("can also contain its", () => false);
                        });
                    });

                    Describe("For each example", () =>
                    {
                        var testCases = new[]
                        {
                            new TestCase(9, 3), 
                            new TestCase(64, 8)
                        };

                        foreach(var testCase in testCases)
                        {                            
                            It("should calculate the square root of " + testCase.Input, () =>
                                GetSquareRoot(testCase.Input) == testCase.Expected);
                        }
                    });
                });
            });
        }

        private int GetSquareRoot(int input)
        {
            return (int)Math.Sqrt(input);
        }
    }

    class TestCase
    {
        public int Input;
        public int Expected;

        public TestCase(int input, int expected)
        {
            Input = input;
            Expected = expected;
        }
    }
}