using MyCompany.Core.Ui;
using MyCompany.Game.Descriptions;
using MyCompany.Game.States;
using MyCompany.Library.Services;
using MyCompany.Library.StateMachine;
using UnityEngine;

namespace MyCompany.Game
{
    public class GameEntryPoint : MonoBehaviour
    {
        [SerializeField] private GameDescription _description;
        [SerializeField] private Camera _camera;
        private void Start()
        {
            ServiceContainer.PermanentServices.Add(new UiController(_description.UiPrefab));
            ServiceContainer.PermanentServices.Add(new MainCamera(_camera));
            var stateMachine = new StateMachine();
            ServiceContainer.PermanentServices.Add(stateMachine);
            ServiceContainer.PermanentServices.Initialize();

            stateMachine.Start(new LoadingGameState(_description));
        }
    }
}
