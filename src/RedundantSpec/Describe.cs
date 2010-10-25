using System;
using System.Collections.Generic;

namespace RedundantSpec
{
    public class Describe
    {
        private List<It> _its = new List<It>();
        public string Name { get; set; }

        public It[] Its
        {
            get { return _its.ToArray(); }
        }

        public Describe[] Describes
        {
            get { return _describes.ToArray(); }
        }

        public Action Before = () => { };
        public Action After = () => { };
        private List<Describe> _describes =new List<Describe>();

        public Describe(string name)
        {
            Name = name;
        }

        public void AddIt(string name, Action action)
        {
            _its.Add(new It(name, action));
        }

        public void AddDescribe(Describe describe)
        {
            _describes.Add(describe);
        }
    }
}