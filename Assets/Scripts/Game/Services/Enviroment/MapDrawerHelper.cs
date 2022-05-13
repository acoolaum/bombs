using MyCompany.Game.Descriptions;
using UnityEngine;

namespace MyCompany.Game.Services
{
    public static class MapDrawerHelper
    {
        public static Vector3 GetWorldCoords(this LevelDescription levelDescription, Vector2Int position)
        {
            return new Vector3(position.x - 0.5f * levelDescription.Size.x + 0.5f, 0f, position.y - 0.5f * levelDescription.Size.y + 0.5f);
        }
        
        public static Vector3 GetWorldCoords(this LevelDescription levelDescription, int x, int y)
        {
            return new Vector3(x - 0.5f * levelDescription.Size.x + 0.5f, 0f, y - 0.5f * levelDescription.Size.y + 0.5f);
        }
        
        public static Vector3 GetSmallCellWorldCoords(this LevelDescription levelDescription, int x, int y)
        {
            return new Vector3(0.2f * x - 0.5f * levelDescription.Size.x + 0.1f, 0f, 0.2f * y - 0.5f * levelDescription.Size.y + 0.1f);
        }
    }
}