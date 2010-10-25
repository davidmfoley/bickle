using System;

namespace RedundantSpec
{
    public interface ITestResultListener
    {
        void Failed(It it, Exception exception);
        void Success(It it);
    }
}