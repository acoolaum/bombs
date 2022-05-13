using MyCompany.Game.Model;

namespace MyCompany.Game.Services
{
    public class ContinuousDamageProcessorCreator : IEffectProcessorCreator
    {
        public IEffectProcessor Create(EffectModelBase effectModel)
        {
            return new ContinuousDamageProcessor((ContinuousDamageEffectModel)effectModel);
        }
    }
}