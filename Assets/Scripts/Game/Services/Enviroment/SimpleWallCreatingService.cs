using MyCompany.Game.Model;
using MyCompany.Library.Services;
using UnityEngine;

namespace MyCompany.Game.Logic
{
    public class SimpleWallCreatingService: ServiceBase
    {
        private LevelModel _levelModel;
        private float _nextTimeSpawn;
        private Vector2Int _mapSize;

        public override void Initialize()
        {
            _levelModel = CurrentServices.GetService<LevelModel>();
            _mapSize = _levelModel.LevelDescription.Size;

            CreateBorderWalls();
            
            CreateInnerWalls();
        }
        private void CreateBorderWalls()
        {
            for (int x = 0; x < _mapSize.x; x++)
            {
                _levelModel.WallCells[x, 0] = true;
                _levelModel.WallCells[x, _mapSize.y - 1] = true;
            }

            for (int y = 1; y < _mapSize.y - 1; y++)
            {
                _levelModel.WallCells[0, y] = true;
                _levelModel.WallCells[_mapSize.x - 1, y] = true;
            }
        }
        private void CreateInnerWalls()
        {
            for (int i = 0; i < _levelModel.LevelDescription.WallDescription.TotalWallsCount; i++)
            {
                int x = Random.Range(1, _mapSize.x - 2);
                int y = Random.Range(1, _mapSize.y - 2);

                if (_levelModel.WallCells[x, y])
                {
                    continue;
                }

                _levelModel.WallCells[x, y] = true;
            }
        }
    }
}