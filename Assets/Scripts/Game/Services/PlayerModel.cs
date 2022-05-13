using System;
using MyCompany.Library.Services;

namespace MyCompany.Game.Services
{
    public class PlayerModel : ServiceBase
    {
        public event Action ModelChanged;
        public int GamePlayedCounter { get; private set; }

        public void IncreaseGamePlayedCounter()
        {
            GamePlayedCounter++;
            ModelChanged?.Invoke();
        }
    }
}