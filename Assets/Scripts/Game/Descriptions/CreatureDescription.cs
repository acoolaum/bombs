using UnityEngine;

namespace MyCompany.Game.Descriptions
{
    [CreateAssetMenu(fileName = "CreatureDescription", menuName = "Descriptions/CreatureDescription")]
    public class CreatureDescription : ScriptableObject
    {
        public int MaxHealth;
        public int MaxSpeed;
        public GameObject Prefab;
    }
}