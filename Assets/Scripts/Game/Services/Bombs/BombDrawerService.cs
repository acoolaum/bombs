using System.Collections.Generic;
using MyCompany.Game.Model;
using MyCompany.Library.Pool;
using MyCompany.Library.Services;
using MyCompany.Library.UnityEvent;
using UnityEngine;

namespace MyCompany.Game.Services
{
    public class BombDrawerService: ServiceBase
    {
        private UnityEventProxy _unityEventProxy;
        private LevelModel _levelModel;
        private Transform _rootTransform;
        
        private readonly List<BombView> _views = new List<BombView>();
        private readonly Dictionary<GameObject, GameObjectPool> _pool = new Dictionary<GameObject, GameObjectPool>();
        
        public override void Initialize()
        {
            _levelModel = CurrentServices.GetService<LevelModel>();
            _levelModel.BombAdded += HandleBombAdded;
            
            _unityEventProxy = CurrentServices.GetService<UnityEventProxy>();
            _unityEventProxy.Updated += HandleUpdated;

            var rootGameObject = new GameObject("Bombs");
            _rootTransform = rootGameObject.transform;
        }

        private void HandleUpdated()
        {
            foreach (var view in _views)
            {
                view.Update();
            }

            foreach (var view in _views)
            {
                if (view.IsExploded)
                {
                    var handler = view.Handler;
                    _pool[view.Model.Description.Prefab].Return(handler);
                }
            }
            
            _views.RemoveAll(view => view.IsExploded);
        }

        private void HandleBombAdded(BombModel bombModel)
        {
            var prefab = bombModel.Description.Prefab;
            
            if (_pool.TryGetValue(prefab, out var pool) == false)
            {
                pool = new GameObjectPool(prefab, _rootTransform.transform, 10);
                _pool.Add(prefab, pool);
            }

            var gameObject = pool.Get();
            
            var view = new BombView(bombModel, _levelModel.LevelDescription);
            view.Show(gameObject);
            _views.Add(view);
        }

        public override void Dispose()
        {
            _levelModel.BombAdded -= HandleBombAdded;
            _unityEventProxy.Updated -= HandleUpdated;
            Object.Destroy(_rootTransform.gameObject);
        }
    }
}