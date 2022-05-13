using MyCompany.Game.Descriptions;
using MyCompany.Library.Services;

namespace MyCompany.Game.Services
{
    public class GameDescriptionAccessor : ServiceBase
    {
        public GameDescription GameDescription { get; }

        public GameDescriptionAccessor(GameDescription gameGameDescription)
        {
            GameDescription = gameGameDescription;
        }
    }
}