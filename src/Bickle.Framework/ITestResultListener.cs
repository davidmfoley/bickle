using System;

namespace Bickle
{
    public interface ITestResultListener
    {
        void Failed(Example example, Exception exception);
        void Success(Example example);
    }
}