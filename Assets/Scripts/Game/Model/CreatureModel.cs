using System;
using System.Collections.Generic;
using MyCompany.Game.Descriptions;
using UnityEngine;

namespace MyCompany.Game.Model
{
    public class CreatureModel : IEffectCarrier, IEffectTarget
    {
        public event Action<EffectModelBase> EffectAssigned;
        public event Action<EffectModelBase> EffectRemoved;
        
        public CreatureDescription Description;
        public Vector2Int Position;
        public string Name => Description.name;
        
        private readonly Dictionary<EffectTargetProperty, CreatureModelProperty> _properties = new Dictionary<EffectTargetProperty, CreatureModelProperty>();
        private readonly List<EffectModelBase> _effects = new List<EffectModelBase>();

        public void AddEffect(EffectModelBase effectModel)
        {
            _effects.Add(effectModel);
            effectModel.Carrier = this;
            effectModel.Target = this;
            EffectAssigned?.Invoke(effectModel);
        }

        public void RemoveEffect(EffectModelBase effectModel)
        {
            _effects.Remove(effectModel);
            EffectRemoved?.Invoke(effectModel);
        }

        public CreatureModelProperty this[EffectTargetProperty targetProperty] => _properties[targetProperty];

        public void AddProperty(CreatureModelProperty property)
        {
            _properties.Add(property.Type, property);
        }
    }
}