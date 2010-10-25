using System;

namespace Bickle
{
    public interface ITestResultListener
    {
        void Failed(It it, Exception exception);
        void Success(It it);
    }
}