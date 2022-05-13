using System;
using MyCompany.Game.Descriptions;

namespace MyCompany.Game.Services
{
    public class BombSpawnController
    {
        public event Action<int, BombDescription> SpawnRequested;

        private readonly BombSpawnDescription _description;

        private float _time;

        public BombSpawnController(BombSpawnDescription description)
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
                SpawnRequested?.Invoke(_description.Amount, _description.BombDescription);
            }
        }
    }
}