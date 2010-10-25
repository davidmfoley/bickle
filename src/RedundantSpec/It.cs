using System;

namespace RedundantSpec
{
    public class It
    {

        private readonly Action _action;
        public string Name;

        public It(string name, Action action)
        {
            Name = name;
            _action = action;
        }

        public void Action()
        {
            _action();
        }
    }
}