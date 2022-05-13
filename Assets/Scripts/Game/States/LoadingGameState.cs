using MyCompany.Core.Ui;
using MyCompany.Game.Descriptions;
using MyCompany.Game.Services;
using MyCompany.Library.Services;
using MyCompany.Library.StateMachine;

namespace MyCompany.Game.States
{
    public class LoadingGameState: StateBase
    {
        private readonly GameDescription _description;
        private readonly UiController _uiController;

        private DialogueWindowController _startGameDialogueWindow;

        public LoadingGameState(GameDescription description)
        {
            _description = description;
            _uiController = ServiceContainer.PermanentServices.GetService<UiController>();
        }

        protected override void OnStarted()
        {
            _startGameDialogueWindow = _uiController.CreateDialogueWindow(_description.DialogueWindowPrefab, _description.GameLoadingMessage);
            _startGameDialogueWindow.ButtonClicked += HandleButtonClicked;
            _startGameDialogueWindow.Show();
        }

        private void HandleButtonClicked()
        {
            _startGameDialogueWindow.Dispose();

            CreateDataLayerServices();
            
            StateMachine.Start(new PlayGameState());
        }

        private void CreateDataLayerServices()
        {
            ServiceContainer.DataServices.Add(new GameDescriptionAccessor(_description));
            ServiceContainer.DataServices.Add(new PlayerModel());
            ServiceContainer.DataServices.Initialize();
        }
    }
}