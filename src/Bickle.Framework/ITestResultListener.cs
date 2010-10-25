using System;

namespace Bickle
{
    public interface ITestResultListener
    {
        void Running(Example example);
        void Failed(Example example, Exception exception);
        void Success(Example example);
        void Finished();
        void Pending(Example example);
    }
}