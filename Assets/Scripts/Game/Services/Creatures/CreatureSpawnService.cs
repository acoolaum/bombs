using System.Collections.Generic;
using MyCompany.Game.Descriptions;
using MyCompany.Game.Model;
using MyCompany.Library.Services;
using MyCompany.Library.UnityEvent;
using UnityEngine;

namespace MyCompany.Game.Services
{
    public class CreatureSpawnService: ServiceBase
    {
        private UnityEventProxy _unityEventProxy;
        private LevelModel _levelModel;
        private List<CreatureSpawnController> _spawnControllers;

        public override void Initialize()
        {
            _levelModel = CurrentServices.GetService<LevelModel>();
            _unityEventProxy = CurrentServices.GetService<UnityEventProxy>();
            _unityEventProxy.FixedUpdated += HandleFixedUpdated;

            _spawnControllers = new List<CreatureSpawnController>(_levelModel.LevelDescription.CreatureSpawnDescriptions.Length);
            foreach (var spawnDescription in _levelModel.LevelDescription.CreatureSpawnDescriptions)
            {
                var spawnController = new CreatureSpawnController(spawnDescription);
                spawnController.SpawnRequested += HandleSpawnRequested;
                _spawnControllers.Add(spawnController);
            }
        }

        private void HandleSpawnRequested(int amount, CreatureDescription creatureDescription)
        {
            for (int i = 0; i < amount; i++)
            {
                Vector2Int randomFreeCell = _levelModel.GetFreeRandomCell();
                if (_levelModel.IsCreatureInCell(randomFreeCell) == false)
                {
                    _levelModel.AddCreature(creatureDescription, randomFreeCell);
                }
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
            _unityEventProxy.FixedUpdated -= HandleFixedUpdated;
        }
    }
}