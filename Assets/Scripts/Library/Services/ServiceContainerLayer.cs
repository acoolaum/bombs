using System;
using System.Collections.Generic;
using System.Linq;
using MyCompany.Library.Utils;
using UnityEngine;

namespace MyCompany.Library.Services
{
    public class ServiceContainerLayer
    {
        public string LayerId { get; }
        public Dictionary<Type, IService> Services;
        private Dictionary<Type, IService> _aliases;
        public bool IsInitialized => _initialized;

        private bool _initialized;

        public ServiceContainerLayer(string layer)
        {
            LayerId = layer;
            Services = new Dictionary<Type, IService>();
            _aliases = new Dictionary<Type, IService>();
        }

        public TService GetService<TService>() where TService : IService
        {
            var service = TryGetService<TService>();
            if (service != null)
            {
                return service;
            }

            throw new KeyNotFoundException($"Service with type {typeof(TService).Name} no found");
        }

        public TService TryGetService<TService>() where TService : IService
        {
            if (Services != null && Services.TryGetValue(typeof(TService), out IService service))
                return (TService) service;
            
            if (_aliases != null && _aliases.TryGetValue(typeof(TService), out IService serviceByAlias))
                return (TService) serviceByAlias;

            return default;
        }


        public void Initialize()
        {
            foreach (var service in Services)
            {
                service.Value.Initialize();
            }

            _initialized = true;
        }

        public void Add<TService>(TService service) where TService : IService
        {
            if (_initialized)
            {
                Debug.LogErrorFormat("Попытка добавления сервиса в уже инициализированный слой '{0}'", LayerId);
                return;
            }

            service.Layer = LayerId;
            Services.Add(typeof(TService), service);
            
            var aliasTypes = typeof(TService).GetBaseTypesAndInterfaces().Where(t =>
            {
                if (typeof(IService).IsAssignableFrom(t) == false ||
                    typeof(IService) == t)
                {
                    return false;
                }

                if (typeof(ServiceBase) == t)
                {
                    return false;
                }

                return true;
            }).ToArray();

            foreach (var alias in aliasTypes)
            {
                _aliases.Add(alias, service);
            }
        }

        public void Dispose()
        {
            foreach (var service in Services)
            {
                service.Value.Dispose();
            }

            Services = null;
            _aliases = null;
        }

        public bool ContainsService<TService>() where TService: IService
        {
            return Services.ContainsKey(typeof(TService)) || _aliases.ContainsKey(typeof(TService));
        }
    }
}