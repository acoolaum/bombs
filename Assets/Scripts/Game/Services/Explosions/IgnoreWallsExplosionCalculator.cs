using MyCompany.Game.Model;
using UnityEngine;

namespace MyCompany.Game.Services
{
    public interface IExplosionCalculator
    {
        void Initialize(ExplosionCalculationBuffer buffer, Vector2Int targetPosition, IWallChecker wallChecker);
        ExplosionCalculationBuffer Calculate();
    }

    public class IgnoreWallsExplosionCalculator: IExplosionCalculator
    {
        protected ExplosionCalculationBuffer Buffer;
        protected Vector2Int TargetPosition;
        protected IWallChecker WallChecker;
        
        public void Initialize(ExplosionCalculationBuffer buffer, Vector2Int targetPosition, IWallChecker wallChecker)
        {
            Buffer = buffer;
            TargetPosition = targetPosition;
            WallChecker = wallChecker;
        }

        protected void InitializeBuffer()
        {
            for (int x = 0; x < Buffer.Size; x++)
            {
                for (int y = 0; y < Buffer.Size; y++)
                {
                    Buffer.Power[x, y] = 0;
                    Buffer.Walls[x, y] = IsWallHere(x, y);
                }
            }
        }

        private bool IsWallHere(int x, int y)
        {
            var mapPositionX = (5 * TargetPosition.x + x - Buffer.Radius + 2) / 5;
            var mapPositionY = (5 * TargetPosition.y + y - Buffer.Radius + 2) / 5;

            return WallChecker.IsWallHere(mapPositionX, mapPositionY);
        }

        public virtual ExplosionCalculationBuffer Calculate()
        {
            InitializeBuffer();
            for (int x = 0; x < Buffer.Size; x++)
            {
                for (int y = 0; y < Buffer.Size; y++)
                {
                    if (Buffer.Walls[x, y] == false)
                    {
                        var distance = Buffer.Distance(x - Buffer.Radius, y - Buffer.Radius);
                        if (distance <= Buffer.Radius)
                        {
                            Buffer.Power[x, y] = ExplosionCalculationBuffer.BasePower * (Buffer.Radius - distance) /
                                                 Buffer.Radius;
                        }
                    }
                }
            }

            return Buffer;
        }
    }
}