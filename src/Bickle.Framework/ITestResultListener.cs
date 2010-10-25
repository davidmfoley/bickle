using System;

namespace Bickle
{
    public interface ITestResultListener
    {
        void Failed(Example example, Exception exception);
        void Success(Example example);
        void Finished();
        void Pending(Example example);
        void Ignored(Example example);
    }
}