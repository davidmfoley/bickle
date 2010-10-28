using System;
using System.Linq.Expressions;

namespace Bickle
{
    public class Example : ExampleNode
    {
        private readonly Action _action;

        public Example(string name, Action action, ExampleContainer parent, Spec spec) : base(parent, name, spec)
        {
            _action = action;
        }

        public Example(string name, Expression<Func<bool>> spec, ExampleContainer parent, Spec containingSpec) : base(parent, name,  containingSpec)
        {
            Name = name;
            _action = BuildAction(spec);
        }

        public Spec Spec;

        


        private static Action BuildAction(Expression<Func<bool>> spec)
        {
            return () =>
            {
                if (!spec.Compile()())
                {
                    throw new AssertionException("Failed:" + SpecDescriber.DescribeSpec(spec));
                }
            };
        }

        public void Action()
        {
            _action();
        }

        public void Execute(ITestResultListener listener)
        {
            try
            {
                if (IsIgnored())
                {
                    listener.Ignored(this);
                    return;
                }
                _action();
                listener.Success(this);
            }
            catch (PendingException)
            {
                listener.Pending(this);
            }
            catch (Exception ex)
            {
                listener.Failed(this, ex);
            }
        }

        
    }
}