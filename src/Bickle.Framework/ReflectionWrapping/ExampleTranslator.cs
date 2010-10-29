using Bickle.Utility;

namespace Bickle.ReflectionWrapping
{
    public class ExampleTranslator
    {
        private readonly ISpec _spec;

        public ExampleTranslator(ISpec spec)
        {
            _spec = spec;
        }

        public IExample Translate(object ex)
        {
            var id = (string)ex.GetPropertyWithReflection("Id");
            return (IExample)_spec.Find(id);
        }
    }
}