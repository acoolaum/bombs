using MyCompany.Game.Descriptions;
using MyCompany.Game.Model;
using MyCompany.Library.Services;
using UnityEngine;

namespace MyCompany.Game.Services
{
    public class ExplosionDamageAssigner : ServiceBase
    {
        private ExplosionCalculationService _explosionCalculationService;
        private LevelModel _levelModel;
        
        public override void Initialize()
        {
            _levelModel = CurrentServices.GetService<LevelModel>();
            _explosionCalculationService = CurrentServices.GetService<ExplosionCalculationService>();
            _explosionCalculationService.Exploded += HandleBombExploded;
        }

        private void HandleBombExploded(ExplosionData explosionData)
        {
            var creatures = _levelModel.GetCreaturesInCircle(explosionData.Bomb.TargetPosition, 1 + explosionData.Bomb.Description.Explosion.Radius / 5);

            var buffer = explosionData.Buffer;
            
            foreach (var creature in creatures)
            {
                var coordInBuffer = 5 * (creature.Position - explosionData.Bomb.TargetPosition) + buffer.Radius * Vector2Int.one;
                if (coordInBuffer.x < 0 ||
                    coordInBuffer.y < 0 ||
                    coordInBuffer.x > buffer.Size - 1 ||
                    coordInBuffer.y > buffer.Size - 1)
                {
                    continue;
                }

                var explosionPowerInPoint = explosionData.Buffer.Power[coordInBuffer.x, coordInBuffer.y];
                if (explosionPowerInPoint > 0)
                {
                    foreach (var damageDescription in explosionData.Bomb.Description.Explosion.DamageDescription)
                    {
                        var damage = explosionPowerInPoint * damageDescription.BaseDamage / ExplosionCalculationBuffer.BasePower;

                        if (damage > damageDescription.MaxDamage)
                        {
                            damage = damageDescription.MaxDamage;
                        }
                        
                        creature.AddEffect(CreateEffect(damageDescription, damage));
                    }
                }
            }
        }

        private EffectModelBase CreateEffect(DamageDescription damageDescription, int damage)
        {
            if (damageDescription.Duration > 0f)
            {
                return new ContinuousDamageEffectModel
                {
                    TargetProperty = damageDescription.TargetProperty,
                    Damage = damage,
                    Duration = damageDescription.Duration
                };
            }
            else
            {
                return new InstantDamageEffectModel
                {
                    TargetProperty = damageDescription.TargetProperty,
                    Damage = damage
                };                
            }
        }
    }
}