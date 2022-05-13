using System;

namespace MyCompany.Library.Services
{
    public interface IService: IDisposable
    {
        string Layer { get; set; }
        void Initialize();
    }
}
