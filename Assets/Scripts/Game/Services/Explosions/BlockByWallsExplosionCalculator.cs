using UnityEngine;

namespace MyCompany.Game.Services
{
    public class BlockByWallsExplosionCalculator: IgnoreWallsExplosionCalculator
    {
        public override ExplosionCalculationBuffer Calculate()
        {
            InitializeBuffer();
            
            Vector2Int center = new Vector2Int(Buffer.Radius, Buffer.Radius);

            for (int x = 0; x < Buffer.Size; x++)
            {
                for (int y = 0; y < Buffer.Size; y++)
                {
                    Vector2Int point = new Vector2Int(x, y);

                    if (point == center)
                    {
                        continue;
                    }

                    Trace(center, point, Buffer);
                }
            }

            return Buffer;
        }

        protected class TraceResult
        {
            public bool Blocked;
            public Vector2Int LastNotBlocked;
            public int Distance;
            public Vector2Int Direction;
        }

        protected static TraceResult Trace(Vector2Int from, Vector2Int point, ExplosionCalculationBuffer buffer,
            int basePower = ExplosionCalculationBuffer.BasePower, int startDistance = 0, PowerOperation powerOperation = PowerOperation.Replace)
        {
            Vector2Int direction = point - from;
            var result = new TraceResult {Direction = direction};

            int max = Mathf.Max(Mathf.Abs(direction.x), Mathf.Abs(direction.y));
            for (int i = 0; i <= max; i++)
            {
                var currentOffset = new Vector2Int(i * direction.x / max, i * direction.y / max);
                var currentPoint = @from + currentOffset;
                if (buffer.Walls[currentPoint.x, currentPoint.y])
                { 
                    result.Blocked = true;
                    return result;
                }

                var distance = buffer.Distance(currentOffset.x, currentOffset.y) + startDistance;
                if (buffer.Radius - distance <= 0)
                {
                    break;
                }

                result.Distance = distance;
                result.LastNotBlocked = currentPoint;

                if (powerOperation == PowerOperation.Replace)
                {
                    var power = (buffer.Radius - distance) * basePower / buffer.Radius;
                    buffer.Power[currentPoint.x, currentPoint.y] = power;
                }
                else
                {
                    var power = (buffer.Radius - distance) * basePower / buffer.Radius;
                    buffer.Power[currentPoint.x, currentPoint.y] += power;
                }
            }

            return result;
        }
        
        protected enum PowerOperation
        {
            Replace,
            Add
        }
    }
}