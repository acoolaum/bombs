using UnityEngine;

namespace MyCompany.Game.Services
{
    public class FastBlockByWallsExplosionCalculator: BlockByWallsExplosionCalculator
    {
        public override ExplosionCalculationBuffer Calculate()
        {
            InitializeBuffer();
            
            Vector2Int center = new Vector2Int(Buffer.Radius, Buffer.Radius);

            for (int t = 0; t < Buffer.Size; t++)
            {
                Trace(center, new Vector2Int(t, 0), Buffer);
                Trace(center, new Vector2Int(t, Buffer.Size - 1), Buffer);
                Trace(center, new Vector2Int(0, t), Buffer);
                Trace(center, new Vector2Int(Buffer.Size - 1, t), Buffer);
            }

            return Buffer;
        }
    }
}