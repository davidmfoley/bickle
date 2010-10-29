using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Bickle
{
    public class Spec : ISpec
    {

        private readonly Stack<IExampleContainer> _describeStack = new Stack<IExampleContainer>();
        private readonly List<IExampleContainer> _describes = new List<IExampleContainer>();
        private Dictionary<string, object> _idMap = new Dictionary<string, object>();

        public static void Pending()
        {
            throw new PendingException();
        }

        protected void Ignore(string area, Action spec)
        {
            var describe = new InactiveExampleContainer(area, CurrentDescribe(), this);
            RunDescribe(spec, describe);
        }

        protected void Ignore(Action spec)
        {
            Ignore("Ignored", spec);
        }

        protected void Describe(string area, Action spec)
        {
            var describe = new ActiveExampleContainer(area, CurrentDescribe(), this);
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
            AddIt(new Example(area, spec, CurrentDescribe(), this));
        }

        protected void It(string area, Expression<Func<bool>> spec)
        {
            AddIt(new Example(area, spec, CurrentDescribe(), this));
        }

        protected void Expect(Expression<Func<bool>> spec)
        {
            AddIt(new Example(SpecDescriber.DescribeSpec(spec), Wrap(spec), CurrentDescribe(), this));
        }

        protected void Specify(Expression<Func<bool>> spec)
        {
            AddIt(new Example(SpecDescriber.DescribeSpec(spec), Wrap(spec), CurrentDescribe(), this));
        }

        private void AddIt(Example example)
        {
            
            CurrentDescribe().AddIt(example);
            _idMap.Add(example.Id, example);
        }

        private Action Wrap(Expression<Func<bool>> spec)
        {
            return () =>
            {
                if (!spec.Compile()())
                    throw new AssertionException(SpecDescriber.DescribeFailure(spec));
            };
        }

        private ExampleContainer CurrentDescribe()
        {
            return (ExampleContainer)(_describeStack.Count > 0 ? _describeStack.Peek() : null);
        }

        private void RunDescribe(Action spec, ExampleContainer exampleContainer)
        {
            if (CurrentDescribeExists())
            {
                exampleContainer.Id = CurrentDescribe().Id + "/" +  CurrentDescribe().ExampleContainers.Length.ToString("000");
                CurrentDescribe().AddDescribe(exampleContainer);
            }
            else
            {
                exampleContainer.Id = GetType().FullName + "/" + _describes.Count.ToString("000");
                _describes.Add(exampleContainer);
            }

            _idMap.Add(exampleContainer.Id, exampleContainer);

            _describeStack.Push(exampleContainer);


            spec();
            _describeStack.Pop();
        }

        private bool CurrentDescribeExists()
        {
            return _describeStack.Count > 0;
        }

       
        public void Execute(ITestResultListener listener)
        {
            foreach (ExampleContainer describe in _describes)
            {
                describe.Execute(listener);
            }
        }

        public object Find(string id)
        {

            return _idMap[id];
        }

        public string Name
        {
            get { return GetType().Name; }
        }

        public bool IsIgnored()
        {
            return !ExampleContainers.Any(x=>!x.IsIgnored());
        }

        public IExampleContainer[] ExampleContainers
        {
            get {  return _describes.ToArray(); }
        }
    }
}