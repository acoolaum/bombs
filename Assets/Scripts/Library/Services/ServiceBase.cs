namespace MyCompany.Library.Services
{
    public abstract class ServiceBase: IService
    {
        public string Layer { get; set; }

        public ServiceContainerLayer PermanentServices => ServiceContainer.PermanentServices;
        public ServiceContainerLayer DataServices => ServiceContainer.DataServices;
        public ServiceContainerLayer CurrentServices => ServiceContainer.GetLayer(Layer);

        public virtual void Initialize() {}
        public virtual void Dispose() {}

        public TService GetService<TService>(string layer) where TService: IService
        {
            return ServiceContainer.GetService<TService>(layer);
        }
    }
}