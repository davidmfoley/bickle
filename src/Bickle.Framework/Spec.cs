using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Bickle;

namespace Bickle
{
    public class Spec
    {
        private List<ExampleContainer> _describes = new List<ExampleContainer>();
        private Action<string, Action> _itHandler = (x, y) => { };
        private Stack<ExampleContainer> _describeStack = new Stack<ExampleContainer>();
               
        protected void Describe(string area, Action spec)
        {
            var describe = new ExampleContainer(area, CurrentDescribe());
            RunDescribe(spec, describe);
           
        }

        protected void Before(Action spec)
        {
            CurrentDescribe().Before = spec;
        }
        
        protected void After(Action spec)
        {
            CurrentDescribe().After = spec;
        }

        protected void It(string area, Action spec)
        {
            CurrentDescribe().AddIt(new Example(area, spec, CurrentDescribe()));
        }

        protected void It(string area, Expression<Func<bool>> spec)
        {
            CurrentDescribe().AddIt(new Example(area, spec, CurrentDescribe()));
        }

        protected void Specify(Expression<Func<bool>> spec)
        {
            CurrentDescribe().AddIt(new Example(SpecDescriber.DescribeSpec(spec), spec, CurrentDescribe()));
        }

        private ExampleContainer CurrentDescribe()
        {
            return _describeStack.Count > 0 ? _describeStack.Peek() : null;
        }

        private void RunDescribe(Action spec, ExampleContainer exampleContainer)
        {
            if (CurrentDescribeExists())
            {
                CurrentDescribe().AddDescribe(exampleContainer);
            }
            else
            {
                _describes.Add(exampleContainer);
            }

            _describeStack.Push(exampleContainer);

            
            spec();
            _describeStack.Pop();
        }

        private bool CurrentDescribeExists()
        {
            return _describeStack.Count > 0;
        }

        public ExampleContainer[] GetSpecs()
        {
            return _describes.ToArray();
        }

        public void Execute(ITestResultListener listener)
        {
            foreach (var describe in _describes)
            {
                describe.Execute(listener);
            }
        }
    }
}
