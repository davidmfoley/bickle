using NUnit.Framework;

namespace RedundantSpec.Tests
{
    public class SpecFor<T> where T : Spec, new()
    {
        protected Describe[] Describes;

        [SetUp]
        public void SetUpContext()
        {
            var spec = new T();

            Describes = spec.GetSpecs();
        }      
    }
}