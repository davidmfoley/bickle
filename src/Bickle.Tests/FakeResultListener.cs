using System;
using System.Collections.Generic;

namespace Bickle.Tests
{
    public class FakeResultListener : ITestResultListener
    {
        public List<string> Calls = new List<string>();

        #region ITestResultListener Members

        public void Failed(IExample example, Exception exception)
        {
            Calls.Add("Failed - " + example.FullName + " - " + exception.GetType().Name);
        }

        public void Success(IExample example)
        {
            Calls.Add("Success - " + example.FullName);
        }

        public void Finished()
        {
        }

        public void Pending(IExample example)
        {
            Calls.Add("Pending - " + example.FullName);
        }

        public void Ignored(IExample example)
        {
            Calls.Add("Ignored - " + example.FullName);
        }

        #endregion

        public void Running(IExample example)
        {
        }
    }
}