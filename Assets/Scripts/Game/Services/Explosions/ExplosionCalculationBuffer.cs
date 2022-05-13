using System.Collections.Generic;
using UnityEngine;

namespace MyCompany.Game.Services
{
    public class ExplosionCalculationBuffer
    {
        public const int BasePower = 1000000;
        public int Radius { get; }
        public int Size { get; }
        public bool[,] Walls { get; }
        public int[,] Power { get; }
        
        private readonly Dictionary<int, int> _distances = new Dictionary<int, int>();

        public ExplosionCalculationBuffer(int radius)
        {
            Radius = radius;
            Size = 2 * radius + 1;
            Walls = new bool[Size,Size];
            Power = new int[Size,Size];

            for (int x = 0; x <= Size; x++)
            {
                for (int y = x; y <= Size; y++)
                {
                    _distances.Add(y * Size + x, Mathf.FloorToInt(Mathf.Sqrt( x * x + y * y)));
                }
            }
        }

        public int Distance(int x, int y)
        {
            int absoluteX = Mathf.Abs(x);
            int absoluteY = Mathf.Abs(y);
            
            if (absoluteX < absoluteY)
            {
                if (_distances.ContainsKey(absoluteY * Size + absoluteX) == false)
                {
                    Debug.Log("!!!");
                }

                return _distances[absoluteY * Size + absoluteX];
            }
            else
            {
                if (_distances.ContainsKey(absoluteX * Size + absoluteY) == false)
                {
                    Debug.Log("!!!");
                }
                
                return _distances[absoluteX * Size + absoluteY];
            }
        }
    }
}