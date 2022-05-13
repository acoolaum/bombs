using UnityEngine;

namespace MyCompany.Game.Descriptions
{
    [CreateAssetMenu(fileName = "CreatureSpawnDescription", menuName = "Descriptions/CreatureSpawnDescription")]
    public class CreatureSpawnDescription: ScriptableObject
    {
        public float StartDelay;
        public float SpawnTimout = 10f;
        public int Amount;
        
        public CreatureDescription CreatureDescription;
    }
}