using MyCompany.Core.Ui;
using MyCompany.Game.Services;
using MyCompany.Library.Services;

namespace MyCompany.Game.States
{
    public class FinishGameState: GameStateBase
    {
        private DialogueWindowController _finishGameDialogueWindow;

        protected override void OnStarted()
        {
            var playerModel = ServiceContainer.DataServices.GetService<PlayerModel>();
            playerModel.IncreaseGamePlayedCounter();
            
            var gameDescription = DescriptionAccessor.GameDescription;
            _finishGameDialogueWindow = UiController.CreateDialogueWindow(
                gameDescription.DialogueWindowPrefab, gameDescription.FinishGameMessage);
            _finishGameDialogueWindow.ButtonClicked += HandleButtonClicked;
            _finishGameDialogueWindow.Show();
        }

        private void HandleButtonClicked()
        {
            StateMachine.Start(new PlayGameState());
        }

        public override void Stop()
        {
            _finishGameDialogueWindow.Dispose();
            ServiceContainer.Dispose(GameLayerName);
        }
    }
}