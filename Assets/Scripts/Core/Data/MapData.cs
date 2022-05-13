using System;
using UnityEngine;

namespace MyCompany.Core.Data
{
    public class MapData<TCellType>
    {
        public struct MapDataElementChangedArgs
        {
            public Vector2Int Position;
            public TCellType OldValue;
            public TCellType NewValue;
        }
        
        public event Action<MapDataElementChangedArgs> ElementChanged;
        public Vector2Int Size { get; }
        
        public TCellType this[int x, int y] {
            get => _map[x, y];
            set
            {
                var oldValue = _map[x, y];
                if (Equals(oldValue, value) == false)
                {
                    _map[x, y] = value;
                    ElementChanged?.Invoke(new MapDataElementChangedArgs
                    {
                        Position = new Vector2Int(x, y),
                        OldValue = oldValue,
                        NewValue = value
                    });
                }
            }
        }

        private readonly TCellType [,] _map;
        
        public MapData(Vector2Int size)
        {
            Size = size;
            _map = new TCellType[size.x, size.y];
        }
    }
}