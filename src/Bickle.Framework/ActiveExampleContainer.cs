using System;

namespace Bickle
{
    public class ActiveExampleContainer : ExampleContainer
    {
        public ActiveExampleContainer(string name, ExampleContainer parent, Spec spec) : base(name, parent, spec)
        {
        }

        public override void Execute(ITestResultListener listener)
        {
            foreach (Example it in Examples)
            {
                foreach (Action before in GetBefores())
                {
                    before();
                }

                it.Execute(listener);

                foreach (Action after in GetAfters())
                {
                    after();
                }
            }

            foreach (ExampleContainer describe in ExampleContainers)
            {
                describe.Execute(listener);
            }
        }
    }
}