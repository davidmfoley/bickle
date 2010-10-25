using System;

namespace Bickle
{
    public class ActiveExampleContainer : ExampleContainer
    {
        public ActiveExampleContainer(string name, ExampleContainer parent) : base(name, parent)
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