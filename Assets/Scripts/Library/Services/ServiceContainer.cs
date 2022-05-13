using System.Collections.Generic;
using UnityEngine;

namespace MyCompany.Library.Services
{
    public class ServiceContainer
    {
        public const string PermanentLayerId = "Permanent";
        public const string DataLayerId = "Data";

        private readonly Dictionary<string, ServiceContainerLayer> _layers = 
            new Dictionary<string, ServiceContainerLayer>();

        private static ServiceContainer _instance;

        public static ServiceContainer Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ServiceContainer();
                }

                return _instance;
            }
        }

        public static ServiceContainerLayer PermanentServices => GetLayerSafe(PermanentLayerId);
        public static ServiceContainerLayer DataServices => GetLayerSafe(DataLayerId);

        private static ServiceContainerLayer GetLayerSafe(string permanentLayerId)
        {
            if (Instance._layers.TryGetValue(permanentLayerId, out var layerServices) == false)
            {
                layerServices = new ServiceContainerLayer(permanentLayerId);
                Instance._layers.Add(permanentLayerId, layerServices);
            }

            return layerServices;
        }


        public static void Add<TService>(string layerId, TService service)  where TService: IService
        {
            if (Instance._layers.TryGetValue(layerId, out var layerServices) == false)
            {
                layerServices = new ServiceContainerLayer(layerId);
                Instance._layers.Add(layerId, layerServices);
            }

            layerServices.Add(service);
        }

        public static TService GetService<TService>(string layerId) where TService : IService
        {
            return Instance._layers[layerId].GetService<TService>();
        }

        public static void InitializeLayer(string layerId)
        {
            var layer = Instance._layers[layerId];
            layer.Initialize();
        }

        public static void Dispose(string layerId)
        {
            if (Instance._layers.TryGetValue(layerId, out var layer) == false)
            {
                Debug.LogError($"Layer not found {layerId}");
                return;
            }

            layer.Dispose();

            Instance._layers.Remove(layerId);
        }

        protected ServiceContainer()
        {

        }

        public static ServiceContainerLayer GetLayer(string layerId)
        {
            return Instance._layers[layerId];
        }
    }
}
