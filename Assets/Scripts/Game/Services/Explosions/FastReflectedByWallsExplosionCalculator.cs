using System.Collections.Generic;
using UnityEngine;

namespace MyCompany.Game.Services
{
    public class FastReflectedByWallsExplosionCalculator: BlockByWallsExplosionCalculator
    {
        public override ExplosionCalculationBuffer Calculate()
        {
            InitializeBuffer();
            
            Vector2Int center = new Vector2Int(Buffer.Radius, Buffer.Radius);
            
            var blocked = new List<TraceResult>();
            
            for (int t = 0; t < Buffer.Size; t++)
            {
                TraceAndCollectBlocked(center, new Vector2Int(t, 0), Buffer, ExplosionCalculationBuffer.BasePower, blocked);
                TraceAndCollectBlocked(center, new Vector2Int(t, Buffer.Size - 1), Buffer, ExplosionCalculationBuffer.BasePower, blocked);
                TraceAndCollectBlocked(center, new Vector2Int(0, t), Buffer, ExplosionCalculationBuffer.BasePower, blocked);
                TraceAndCollectBlocked(center, new Vector2Int(Buffer.Size - 1, t), Buffer, ExplosionCalculationBuffer.BasePower, blocked);
            }
            
            var reflectedRayPower = ExplosionCalculationBuffer.BasePower / Buffer.Radius;
            foreach (var traceResult in blocked)
            {
                var currentTraceResult = traceResult;
                
                int reflectionCount = 0;
                while (currentTraceResult.Blocked && reflectionCount < 3)
                {
                    var direction = traceResult.Direction;
                    if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                    {
                        direction.x = -direction.x;
                    }
                    else
                    {
                        direction.y = -direction.y;
                    }

                    currentTraceResult = Trace(traceResult.LastNotBlocked, direction, Buffer, reflectedRayPower,
                        traceResult.Distance, PowerOperation.Add);
                    reflectionCount++;
                }
            }

            return Buffer;
        }

        private static void TraceAndCollectBlocked(Vector2Int @from, Vector2Int to, ExplosionCalculationBuffer buffer,
            int basePower, List<TraceResult> blocked)
        {
            var traceResult = Trace(from, to, buffer, basePower);
            if (traceResult.Blocked)
            {
                blocked.Add(traceResult);
            }
        }
    }
}