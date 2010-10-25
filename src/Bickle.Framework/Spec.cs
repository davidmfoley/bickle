using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace Bickle
{
    public class Spec
    {
        private readonly Stack<ExampleContainer> _describeStack = new Stack<ExampleContainer>();
        private readonly List<ExampleContainer> _describes = new List<ExampleContainer>();

        public static void Pending()
        {
            throw new PendingException();
        }

        protected void Ignore(string area, Action spec)
        {
            var describe = new InactiveExampleContainer(area, CurrentDescribe());
            RunDescribe(spec, describe);
        }

        protected void Ignore(Action spec)
        {
            Ignore("Ignored", spec);
        }

        protected void Describe(string area, Action spec)
        {
            var describe = new ActiveExampleContainer(area, CurrentDescribe());
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

        protected void Expect(Expression<Func<bool>> spec)
        {
            CurrentDescribe().AddIt(new Example(SpecDescriber.DescribeSpec(spec), Wrap(spec), CurrentDescribe()));
        }

        protected void Specify(Expression<Func<bool>> spec)
        {
            CurrentDescribe().AddIt(new Example(SpecDescriber.DescribeSpec(spec), Wrap(spec), CurrentDescribe()));
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
            foreach (ExampleContainer describe in _describes)
            {
                describe.Execute(listener);
            }
        }
    }

    [Serializable]
    public class PendingException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public PendingException()
        {
        }

        public PendingException(string message) : base(message)
        {
        }

        public PendingException(string message, Exception inner) : base(message, inner)
        {
        }

        protected PendingException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}