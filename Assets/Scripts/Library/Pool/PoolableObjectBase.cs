namespace MyCompany.Library.Pool
{
    public abstract class PoolableObjectBase<T> : IPoolableHandler<T> where T : IPoolableHandler<T>
    {
        public void Return()
        {
            OnReturn();
        }

        public void Get()
        {
            OnGet();
        }
        public abstract void Dispose();
        protected abstract void OnGet();
        protected abstract void OnReturn();
    }
}