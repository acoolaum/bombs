namespace MyCompany.Game.Model
{
    public interface IEffectCarrier
    {
        void AddEffect(EffectModelBase effectModel);
        void RemoveEffect(EffectModelBase effectModel);
        string Name { get; }
    }
}