using System;

namespace Bickle
{
    public interface ITestResultListener
    {
        void Failed(IExample example, Exception exception);
        void Success(IExample example);
        void Finished();
        void Pending(IExample example);
        void Ignored(IExample example);
    }
}