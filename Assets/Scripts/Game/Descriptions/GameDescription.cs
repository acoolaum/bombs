using MyCompany.Core.Descriptions;
using UnityEngine;

namespace MyCompany.Game.Descriptions
{
    [CreateAssetMenu(fileName = "GameDescription", menuName = "Descriptions/GameDescription")]
    public class GameDescription: GameDescriptionBase
    {
        public GameObject DialogueWindowPrefab;
        public GameObject DamageViewPrefab;

        public string GameLoadingMessage = "Game loading...";
        public string FinishGameMessage = "Game over";
        
        public LevelDescription[] Levels;
    }
}