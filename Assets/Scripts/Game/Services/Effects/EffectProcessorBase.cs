using MyCompany.Game.Model;

namespace MyCompany.Game.Services
{
    public abstract class EffectProcessorBase<TEffect> : IEffectProcessor where TEffect : EffectModelBase
    {
        EffectModelBase IEffectProcessor.EffectModel => Model;
        protected TEffect Model { get; }
        
        public EffectProcessorBase(TEffect model)
        {
            Model = model;
        }

        public abstract bool Process(float deltaTime);

        public void Remove()
        {
            Model.Carrier.RemoveEffect(Model);
        }
    }
}