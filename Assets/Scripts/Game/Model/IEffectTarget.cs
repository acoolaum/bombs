using MyCompany.Game.Descriptions;

namespace MyCompany.Game.Model
{
    public interface IEffectTarget
    {
        CreatureModelProperty this[EffectTargetProperty targetProperty] { get; }
    }
}