using System;
using MyCompany.Game.Descriptions;

namespace MyCompany.Game.Services
{
    public class CreatureSpawnController
    {
        public event Action<int, CreatureDescription> SpawnRequested;

        private readonly CreatureSpawnDescription _description;

        private float _time;

        public CreatureSpawnController(CreatureSpawnDescription description)
        {
            _description = description;
            _time = _description.StartDelay;
        }

        public void Process(float fixedDeltaTime)
        {
            _time -= fixedDeltaTime;

            while (_time <= 0f)
            {
                _time += _description.SpawnTimout;
                SpawnRequested?.Invoke(_description.Amount, _description.CreatureDescription);
            }
        }
    }
}