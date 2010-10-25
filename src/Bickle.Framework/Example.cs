using System;
using System.Linq.Expressions;

namespace Bickle
{
    public class Example
    {
        private readonly Action _action;
        private readonly ExampleContainer _parent;
        public string Name;

        public string FullName
        {
            get
            {
                return _parent.FullName + ", " + Name;
            }
        }

        public Example(string name, Action action, ExampleContainer parent)
        {
            Name = name;
            _action = action;
            _parent = parent;
        }

        public Example(string name, Expression<Func<bool>> spec, ExampleContainer parent)
        {
            Name = name;
            _action = BuildAction(spec);
            _parent = parent;
        }

        private Action BuildAction(Expression<Func<bool>> spec)
        {
            return ()=>
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
                _action();
                listener.Success(this);
            }
            catch(Exception ex)
            {
                listener.Failed(this, ex);
            }
        }
    }
}