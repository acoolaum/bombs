using System;
using System.Collections.Generic;
using System.Diagnostics;
using MyCompany.Game.Descriptions;
using MyCompany.Game.Model;
using MyCompany.Library.Services;
using MyCompany.Library.UnityEvent;
using Debug = UnityEngine.Debug;

namespace MyCompany.Game.Services
{
    public class ExplosionCalculationService : ServiceBase
    {
        public event Action<ExplosionData> Exploded;
        private readonly List<BombModel> _explodedBombs = new List<BombModel>();

        private UnityEventProxy _unityEventProxy;
        private LevelModel _levelModel;
        private Dictionary<int, ExplosionCalculationBuffer> _buffers;

        public override void Initialize()
        {
            _levelModel = CurrentServices.GetService<LevelModel>();
            _levelModel.BombExploded += HandleBombExploded;

            _unityEventProxy = CurrentServices.GetService<UnityEventProxy>();
            _unityEventProxy.FixedUpdated += HandleFixedUpdated;

            _buffers = new Dictionary<int, ExplosionCalculationBuffer>();
        }

        private void HandleBombExploded(BombModel model)
        {
            _explodedBombs.Add(model);
        }

        private void HandleFixedUpdated()
        {
            foreach (var bomb in _explodedBombs)
            {
                AssignBombDamage(bomb);
            }

            _explodedBombs?.Clear();
        }

        private void AssignBombDamage(BombModel bombModel)
        {
            var calculator = CreateExplosionCalculator(bombModel);
            var buffer = calculator.Calculate();
            
            Exploded?.Invoke(new ExplosionData {Bomb = bombModel, Buffer = buffer});
        }

        private IExplosionCalculator CreateExplosionCalculator(BombModel bombModel)
        {
            var explosionDescription = bombModel.Description.Explosion;
            var radius = explosionDescription.Radius;

            if (_buffers.TryGetValue(radius, out var buffer) == false)
            {
                buffer = new ExplosionCalculationBuffer(radius);
                _buffers.Add(radius, buffer);
            }

            IExplosionCalculator calculator;
            
            switch (bombModel.Description.Explosion.ExplosionType)
            {
                case ExplosionType.ReflectedByWalls:
                    calculator = new FastReflectedByWallsExplosionCalculator();
                    break;
                case ExplosionType.BlockedByWalls:
                    calculator = new FastBlockByWallsExplosionCalculator();
                    break;
                default:
                    calculator = new IgnoreWallsExplosionCalculator();
                    break;
            }
            
            calculator.Initialize(buffer, bombModel.TargetPosition, _levelModel);
            return calculator;
        }

        public override void Dispose()
        {
            _unityEventProxy.Updated -= HandleFixedUpdated;
            _levelModel.BombExploded -= HandleBombExploded;
        }
    }
}