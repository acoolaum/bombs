using MyCompany.Game.Descriptions;
using UnityEngine;

namespace MyCompany.Game.Model
{
    public class BombModel
    {
        public BombDescription Description;
        public Vector2Int StartPosition;
        public Vector2Int TargetPosition;
        public float CurrentTime;
        public float TotalMoveDuration;
        public bool IsExploded;
        public bool Changed;
    }
}