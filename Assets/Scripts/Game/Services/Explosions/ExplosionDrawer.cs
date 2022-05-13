using System.Collections.Generic;
using DG.Tweening;
using MyCompany.Game.Model;
using MyCompany.Library.Pool;
using MyCompany.Library.Services;
using UnityEngine;

namespace MyCompany.Game.Services
{
    public class ExplosionDrawer : ServiceBase
    {
        private ExplosionCalculationService _explosionCalculationService;
        private LevelModel _levelModel;
        private GameObject _rootGameObject;
        private Dictionary<GameObject, GameObjectPool> _explosionPool;
        private List<Sequence> _returnTweens = new List<Sequence>();

        public override void Initialize()
        {
            _levelModel = CurrentServices.GetService<LevelModel>();
            _explosionCalculationService = CurrentServices.GetService<ExplosionCalculationService>();
            _explosionCalculationService.Exploded += HandleBombExploded;
            
            _rootGameObject = new GameObject("ExplosionsRoot");
            
            _explosionPool = new Dictionary<GameObject, GameObjectPool>(1);
        }
        
        public override void Dispose()
        {
            _explosionCalculationService.Exploded -= HandleBombExploded;
            foreach (var tween in _returnTweens)
            {
                tween.Kill();
            }
            Object.Destroy(_rootGameObject);
        }

        private void HandleBombExploded(ExplosionData explosionData)
        {
            var levelDescription = _levelModel.LevelDescription;
            var prefab = explosionData.Bomb.Description.ExplosionPrefab;
            
            if (_explosionPool.TryGetValue(prefab, out var pool) == false)
            {
                pool = new GameObjectPool(prefab, _rootGameObject.transform, 10);
                _explosionPool.Add(prefab, pool);
            }

            var handler = pool.Get();
            handler.GameObject.transform.position = levelDescription.GetWorldCoords(explosionData.Bomb.TargetPosition);

            var sequence = DOTween.Sequence();
            sequence.Append(DOVirtual.DelayedCall(1.5f , () =>
            {
                pool.Return(handler);
                _returnTweens.Remove(sequence);
            }));
            _returnTweens.Add(sequence);
        }
    }
}