﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Bickle
{
    public class Describe
    {
        private readonly Describe _parent;
        private List<Example> _its = new List<Example>();
        public string Name { get; set; }

        public Example[] Examples
        {
            get { return _its.ToArray(); }
        }

        public Describe[] Describes
        {
            get { return _describes.ToArray(); }
        }

        public Action Before = () => { };
        public Action After = () => { };
        private readonly List<Describe> _describes = new List<Describe>();
        
        public string FullName
        {
            get { return (_parent != null ? _parent.FullName + ", " : "") + Name; }
        }

        public Describe(string name, Describe parent)
        {
            _parent = parent;
            Name = name;
        }

        public void AddIt(Example example)
        {
            _its.Add(example);
        }

        public void AddDescribe(Describe describe)
        {
            _describes.Add(describe);
        }

        public void Execute()
        {
            foreach (var it in Examples)
            {
                foreach (var before in GetBefores())
                {
                    before();
                }

                it.Action();

                foreach (var after in GetAfters())
                {
                    after();
                }
            }

            foreach (var describe in Describes)
            {
                describe.Execute();
            }
        }

        private IEnumerable<Action> GetBefores()
        {
            if (_parent == null)
                return new[] { Before };

           return  new[] {Before}.Union(_parent.GetBefores());
        }

        private IEnumerable<Action> GetAfters()
        {
            if (_parent == null)
                return new[] {After};

            return (new[] { After }.Union(_parent.GetAfters())).Reverse();
        }
    }
}