using System;
using UnityEngine;

namespace MyCompany.Game.Descriptions
{
    [CreateAssetMenu(fileName = "LevelDescription", menuName = "Descriptions/LevelDescription")]
    public class LevelDescription : ScriptableObject
    {
        public Vector2Int Size;
        
        public GameObject [] FloorPrefabs;
        public LevelWallDescription WallDescription;
        public CreatureSpawnDescription[] CreatureSpawnDescriptions;
        public BombSpawnDescription[] BombSpawnDescriptions;
        public float GameDuration = 100;
    }

    [Serializable]
    public class LevelWallDescription
    {
        public int TotalWallsCount;
        public GameObject WallPrefab;
        public float MaxShowDelay = 3f;
        public float MoveSpeed = 10f;
    }
}