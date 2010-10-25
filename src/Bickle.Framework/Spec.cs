using System;
using System.Collections.Generic;

namespace RedundantSpec
{
    public class Spec
    {
        private List<Describe> _describes = new List<Describe>();
        private Action<string, Action> _itHandler = (x, y) => { };
        private Stack<Describe> _describeStack = new Stack<Describe>();
               
        protected void Describe(string area, Action spec)
        {
            var describe = new Describe(area, CurrentDescribe());
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
            CurrentDescribe().AddIt(area, spec);
        }

        private Describe CurrentDescribe()
        {
            return _describeStack.Count > 0 ? _describeStack.Peek() : null;
        }

        private void RunDescribe(Action spec, Describe describe)
        {
            if (CurrentDescribeExists())
            {
                CurrentDescribe().AddDescribe(describe);
            }
            else
            {
                _describes.Add(describe);
            }

            _describeStack.Push(describe);

            
            spec();
            _describeStack.Pop();
        }

        private bool CurrentDescribeExists()
        {
            return _describeStack.Count > 0;
        }

        public Describe[] GetSpecs()
        {
            return _describes.ToArray();
        }

        public void Execute(ITestResultListener listener)
        {
            foreach (var describe in _describes)
            {
                describe.Execute();
            }
        }
    }
}
