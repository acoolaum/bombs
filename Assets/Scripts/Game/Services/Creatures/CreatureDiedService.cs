using System.Collections.Generic;
using System.Linq;
using MyCompany.Game.Descriptions;
using MyCompany.Game.Model;
using MyCompany.Library.Services;
using MyCompany.Library.UnityEvent;

namespace MyCompany.Game.Services
{
    public class CreatureDiedService: ServiceBase
    {
        private UnityEventProxy _unityEventProxy;
        private LevelModel _levelModel;
        private List<CreatureModel> _models;

        public override void Initialize()
        {
            _levelModel = CurrentServices.GetService<LevelModel>();
            _levelModel.CreatureAdded += HandleCreatureAdded;
            _levelModel.CreatureRemoved += HandleCreatureRemoved;
            
            _unityEventProxy = CurrentServices.GetService<UnityEventProxy>();
            _unityEventProxy.Updated += HandleFixedUpdated;
            
            _models = new List<CreatureModel>();
        }

        private void HandleCreatureRemoved(CreatureModel model)
        {
            _models.Remove(model);
        }

        private void HandleCreatureAdded(CreatureModel model)
        {
            _models.Add(model);
        }

        private void HandleFixedUpdated()
        {
            var dead = _models.Where(m => m[EffectTargetProperty.Health].Value == 0).ToArray();
            foreach (var deadCreature in dead)
            {
                _levelModel.RemoveCreature(deadCreature);
            }
        }

        public override void Dispose()
        {
            _levelModel.CreatureAdded -= HandleCreatureAdded;
            _levelModel.CreatureRemoved -= HandleCreatureRemoved;
            
            _unityEventProxy.Updated -= HandleFixedUpdated;
        }
    }
}