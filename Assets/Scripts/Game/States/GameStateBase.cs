using MyCompany.Core.Ui;
using MyCompany.Game.Services;
using MyCompany.Library.Services;
using MyCompany.Library.StateMachine;

namespace MyCompany.Game.States
{
    public abstract class GameStateBase: StateBase
    {
        public const string GameLayerName = "GameLayer";
        
        protected UiController UiController;
        protected GameDescriptionAccessor DescriptionAccessor;
        protected PlayerModel PlayerModel;

        protected GameStateBase()
        {
            UiController = ServiceContainer.PermanentServices.GetService<UiController>();
            DescriptionAccessor = ServiceContainer.DataServices.GetService<GameDescriptionAccessor>();
            PlayerModel = ServiceContainer.DataServices.GetService<PlayerModel>();
        }
    }
}