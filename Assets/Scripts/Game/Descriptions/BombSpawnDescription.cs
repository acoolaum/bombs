using UnityEngine;

namespace MyCompany.Game.Descriptions
{
    [CreateAssetMenu(fileName = "BombSpawnDescription", menuName = "Descriptions/BombSpawnDescription")]
    public class BombSpawnDescription: ScriptableObject, ISpawnDescription<BombDescription>
    {
        public float StartDelay;
        public float SpawnTimout = 10f;
        public int Amount = 1;
        
        public BombDescription BombDescription;
        
        public BombDescription ItemDescription => BombDescription;
    }

    public interface ISpawnDescription<out TDescription>
    {
        TDescription ItemDescription { get; }
    }
}