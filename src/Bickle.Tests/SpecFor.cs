using NUnit.Framework;

namespace Bickle.Tests
{
    public class SpecFor<T> where T : Spec, new()
    {
        protected ExampleContainer[] ExampleContainers;

        [SetUp]
        public void SetUpContext()
        {
            var spec = new T();

            ExampleContainers = spec.GetSpecs();
        }
    }
}