using System;

namespace MyCompany.Library.Pool
{
    public interface IPoolableHandler<T> : IDisposable
    {
        void Return();
        void Get();
    }
}