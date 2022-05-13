using System.Collections.Generic;
using MyCompany.Game.Descriptions;
using MyCompany.Game.Model;
using MyCompany.Library.Services;
using MyCompany.Library.UnityEvent;
using UnityEngine;

namespace MyCompany.Game.Services
{
    public class BombSpawnService: ServiceBase
    {
        private UnityEventProxy _unityEventProxy;
        private LevelModel _levelModel;
        private List<BombSpawnController> _spawnControllers;

        public override void Initialize()
        {
            _levelModel = CurrentServices.GetService<LevelModel>();
            _unityEventProxy = CurrentServices.GetService<UnityEventProxy>();
            _unityEventProxy.FixedUpdated += HandleFixedUpdated;

            _spawnControllers = new List<BombSpawnController>(_levelModel.LevelDescription.BombSpawnDescriptions.Length);
            foreach (var spawnDescription in _levelModel.LevelDescription.BombSpawnDescriptions)
            {
                var spawnController = new BombSpawnController(spawnDescription);
                spawnController.SpawnRequested += HandleSpawnRequested;
                _spawnControllers.Add(spawnController);
            }
        }

        private void HandleSpawnRequested(int amount, BombDescription bombDescription)
        {
            for (int i = 0; i < amount; i++)
            {
                Vector2Int randomFreeCell = _levelModel.GetFreeRandomCell();
                _levelModel.AddBomb(bombDescription, randomFreeCell);
            }
        }

        private void HandleFixedUpdated()
        {
            foreach (var spawnController in _spawnControllers)
            {
                spawnController.Process(Time.fixedDeltaTime);
            }
        }

        public override void Dispose()
        {
            _unityEventProxy.Updated -= HandleFixedUpdated;
        }
    }
}