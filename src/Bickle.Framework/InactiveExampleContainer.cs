namespace Bickle
{
    public class InactiveExampleContainer : ExampleContainer
    {
        public InactiveExampleContainer(string name, ExampleContainer parent, Spec spec) : base(name, parent, spec)
        {
        }

        public override void Execute(ITestResultListener listener)
        {
            ExecuteIgnored(this, listener);
        }

        private void ExecuteIgnored(ExampleContainer container, ITestResultListener listener)
        {
            foreach (ExampleContainer exampleContainer in container.ExampleContainers)
            {
                ExecuteIgnored(exampleContainer, listener);
            }

            foreach (Example example in container.Examples)
            {
                listener.Ignored(example);
            }
        }
    }
}