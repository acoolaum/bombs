using System;
using System.Collections.Generic;
namespace MyCompany.Library.Pool
{
    public class Pool<TPoolableHandler> : IDisposable where TPoolableHandler : IPoolableHandler<TPoolableHandler>
    {
        private List<TPoolableHandler> _pool;
        private Func<TPoolableHandler> _createMethod;

        public TPoolableHandler Get()
        {
            TPoolableHandler result;
            if (_pool.Count == 0)
            {
                result = _createMethod.Invoke();
            }
            else
            {
                result = _pool[_pool.Count - 1];
                _pool.RemoveAt(_pool.Count - 1);
            }
            result.Get();
            return result;
        }

        public void Return(TPoolableHandler poolable)
        {
            poolable.Return();
            _pool.Add(poolable);
        }
        
        public void Dispose()
        {
            foreach (var poolable in _pool)
            {
                poolable.Dispose();
            }
        }
        
        protected void SetCreateMethod(Func<TPoolableHandler> createMethod)
        {
            _createMethod = createMethod;
        }

        protected void Initialize(int initialSize)
        {
            _pool = new List<TPoolableHandler>(initialSize);
            for (int i = 0; i < initialSize; i++)
            {
                var newHandler = _createMethod.Invoke();
                _pool.Add(newHandler);
            }
        }
    }
}