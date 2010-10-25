using System;
using System.Collections.Generic;

namespace RedundantSpec
{
    public class It
    {
        private readonly Action _action;
        private readonly Describe _parent;
        public string Name;

        public string FullName { get { var names = new List<string>();
            return _parent.FullName + ", " + Name;
        }}

        public It(string name, Action action, Describe parent)
        {
            Name = name;
            _action = action;
            _parent = parent;
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