using MyCompany.Game.Model;
using UnityEngine;

namespace MyCompany.Game.Services
{
    public class ContinuousDamageProcessor : EffectProcessorBase<ContinuousDamageEffectModel>
    {
        public ContinuousDamageProcessor(ContinuousDamageEffectModel model) : base(model)
        {
        }

        public override bool Process(float deltaTime)
        {
            Model.CurrentTime += deltaTime;

            var lastIteration = Model.CurrentTime >= Model.Duration;
            
            var totalDamage = lastIteration
                ? Model.Damage
                : Mathf.FloorToInt(Model.CurrentTime * Model.Damage / Model.Duration);

            var damage = totalDamage - Model.DamageDone;
            
            var property = Model.Target[Model.TargetProperty];
            property.Value -= Mathf.Min(property.Value, damage);

            Model.DamageDone = totalDamage;
            
            return lastIteration == false;
        }
    }
}