using MyCompany.Game.Model;

namespace MyCompany.Game.Services
{
    public class InstantDamageProcessorCreator : IEffectProcessorCreator
    {
        public IEffectProcessor Create(EffectModelBase effectModel)
        {
            return new InstantDamageProcessor((InstantDamageEffectModel)effectModel);
        }
    }
}