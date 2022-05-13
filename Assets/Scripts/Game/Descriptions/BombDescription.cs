using System;
using UnityEngine;

namespace MyCompany.Game.Descriptions
{
    [CreateAssetMenu(fileName = "BombDescription", menuName = "Descriptions/BombDescription")]
    public class BombDescription : ScriptableObject
    {
        public float Speed;

        public ExplosionDescription Explosion;
            
        public GameObject Prefab;
        public GameObject ExplosionPrefab;
    }
    
    [Serializable]
    public class ExplosionDescription
    {
        public ExplosionType ExplosionType;
        public int Radius;
        public DamageDescription [] DamageDescription;
    }

    [Serializable]
    public class DamageDescription
    {
        public EffectTargetProperty TargetProperty;  
        public int BaseDamage;
        public int MaxDamage;
        public float Duration;
    }

    public enum ExplosionType
    {
        IgnoreWalls,
        BlockedByWalls,
        ReflectedByWalls
    }

    public enum EffectTargetProperty
    {
        Health,
        Speed
    }
}