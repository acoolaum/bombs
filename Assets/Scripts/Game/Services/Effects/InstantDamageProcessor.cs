using MyCompany.Game.Descriptions;
using MyCompany.Game.Model;
using UnityEngine;

namespace MyCompany.Game.Services
{
    public class InstantDamageProcessor : EffectProcessorBase<InstantDamageEffectModel>
    {
        public InstantDamageProcessor(InstantDamageEffectModel model) : base(model)
        {
        }

        public override bool Process(float deltaTime)
        {
            var property = Model.Target[Model.TargetProperty];
            property.Value -= Mathf.Min(property.Value, Model.Damage);
            return false;
        }
    }
}