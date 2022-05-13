using System.Collections.Generic;
using System.Linq;
using MyCompany.Game.Model;
using MyCompany.Library.Services;
using MyCompany.Library.UnityEvent;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MyCompany.Game.Services
{
    public class CreatureDrawerService: ServiceBase
    {
        private UnityEventProxy _unityEventProxy;
        private LevelModel _levelModel;
        private Transform _creatureRootTransform;
        
        private readonly List<CreatureView> _creatureViews = new List<CreatureView>();
        private readonly List<CreatureView> _markedForDead = new List<CreatureView>();

        public override void Initialize()
        {
            _levelModel = CurrentServices.GetService<LevelModel>();
            _levelModel.CreatureAdded += HandleCreatureAdded;
            _levelModel.CreatureRemoved += HandleCreatureRemoved;

            _unityEventProxy = CurrentServices.GetService<UnityEventProxy>();
            _unityEventProxy.Updated += HandleUpdated;

            var rootGameObject = new GameObject("Creatures");
            _creatureRootTransform = rootGameObject.transform;
        }

        private void HandleUpdated()
        {
            if (_markedForDead.Count > 0)
            {
                foreach (var creature in _markedForDead)
                {
                    _creatureViews.Remove(creature);
                    creature.Dispose();
                }
            }

            foreach (var view in _creatureViews)
            {
                view.Update();
            }
        }

        private void HandleCreatureAdded(CreatureModel creatureModel)
        {
            var creatureView = new CreatureView(creatureModel, _creatureRootTransform, _levelModel.LevelDescription);
            creatureView.Show();
            _creatureViews.Add(creatureView);
        }
        
        private void HandleCreatureRemoved(CreatureModel model)
        {
            var creatureView = _creatureViews.First(view => view.Model == model);
            _markedForDead.Add(creatureView);
        }

        public override void Dispose()
        {
            Object.Destroy(_creatureRootTransform.gameObject);
            
            _levelModel.CreatureAdded -= HandleCreatureAdded;
            _levelModel.CreatureRemoved -= HandleCreatureRemoved;
            
            _unityEventProxy.Updated -= HandleUpdated;
        }

        public Vector3 GetCreatureTooltipPosition(CreatureModel model)
        {
            var creatureView = _creatureViews.First(c => c.Model == model);
            return creatureView.GetTooltipPosition();
        }
    }
}