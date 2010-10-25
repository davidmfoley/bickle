﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Bickle
{
    public class Example
    {
        private readonly Action _action;
        private readonly Describe _parent;
        public string Name;

        public string FullName { get { var names = new List<string>();
            return _parent.FullName + ", " + Name;
        }}

        public Example(string name, Action action, Describe parent)
        {
            Name = name;
            _action = action;
            _parent = parent;
        }

        public Example(string name, Expression<Func<bool>> spec, Describe parent)
        {
            Name = name;
            _action = BuildAction(spec);
            _parent = parent;
        }

        private Action BuildAction(Expression<Func<bool>> spec)
        {
            return null;
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