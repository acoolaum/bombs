namespace MyCompany.Game.Model
{
    public abstract class EffectModelBase
    {
        public IEffectCarrier Carrier { get; set; }
        public IEffectTarget Target { get; set; }
    }
}