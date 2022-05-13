using System.Collections.Generic;
using MyCompany.Game.Model;
using MyCompany.Library.Services;
using MyCompany.Library.UnityEvent;
using UnityEngine;

namespace MyCompany.Game.Services
{
    public class BombMovementService: ServiceBase
    {
        private readonly List<BombController> _controllers = new List<BombController>();
        
        private UnityEventProxy _unityEventProxy;
        private LevelModel _levelModel;
        public override void Initialize()
        {
            _levelModel = CurrentServices.GetService<LevelModel>();
            _levelModel.BombAdded += HandleBombAdded;
            _unityEventProxy = CurrentServices.GetService<UnityEventProxy>();
            _unityEventProxy.FixedUpdated += HandleFixedUpdated;
        }

        private void HandleBombAdded(BombModel model)
        {
            var controller = new BombController(model);
            controller.Exploded += HandleBombExploded;
            _controllers.Add(controller);
        }

        private void HandleBombExploded(BombModel model)
        {
            _levelModel.ExplodeBomb(model);
        }

        private void HandleFixedUpdated()
        {
            foreach (var controller in _controllers)
            {
                controller.Process(Time.fixedDeltaTime);
            }

            _controllers.RemoveAll(controller => controller.IsExploded);
        }

        public override void Dispose()
        {
            _unityEventProxy.Updated -= HandleFixedUpdated;
            _levelModel.BombAdded -= HandleBombAdded;
        }
    }
}