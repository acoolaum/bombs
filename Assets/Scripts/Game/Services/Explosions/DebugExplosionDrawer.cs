using MyCompany.Game.Descriptions;
using MyCompany.Game.Model;
using MyCompany.Library.Services;
using UnityEngine;

namespace MyCompany.Game.Services
{
    public class DebugExplosionDrawer : ServiceBase
    {
        private ExplosionCalculationService _explosionCalculationService;
        private LevelModel _levelModel;
        private Renderer[,] _damageCells;
        private GameObject _rootGameObject;

        public override void Initialize()
        {
            _levelModel = CurrentServices.GetService<LevelModel>();
            _explosionCalculationService = CurrentServices.GetService<ExplosionCalculationService>();
            _explosionCalculationService.Exploded += HandleBombExploded;
            
            _rootGameObject = new GameObject("DamageCellsRoot");
            
            DrawDamageCells(_levelModel.LevelDescription);
        }

        public override void Dispose()
        {
            _explosionCalculationService.Exploded -= HandleBombExploded;
            Object.Destroy(_rootGameObject);
        }

        private void DrawDamageCells(LevelDescription levelDescription)
        {
            var rootTransform = _rootGameObject.transform;
            var size = levelDescription.Size;
            
            _damageCells = new Renderer[5 * size.x, 5 * size.y];
           
            for (int x = 0; x < 5 * size.x; x++)
            {
                for (int y = 0; y < 5 * size.y; y++)
                {
                    var prefab = levelDescription.FloorPrefabs[(x + y) % 2];
                    var gameObject = Object.Instantiate(prefab, levelDescription.GetSmallCellWorldCoords(x, y) + 0.0001f * Vector3.up, Quaternion.identity, rootTransform);
                    
                    gameObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                    _damageCells[x, y] = gameObject.GetComponentInChildren<MeshRenderer>();
                }
            }
        }

        private void HandleBombExploded(ExplosionData explosionData)
        {
            var buffer = explosionData.Buffer;
            var bombModel = explosionData.Bomb;
            
            for (int x = 0; x < buffer.Size; x++)
            {
                for (int y = 0; y < buffer.Size; y++)
                {
                    SetDamageCellColor(
                        new Color((float) buffer.Power[x, y] / ExplosionCalculationBuffer.BasePower / 2, 0f, 0f, 1f),
                        x - buffer.Radius + bombModel.TargetPosition.x * 5 + 2,
                        y - buffer.Radius + bombModel.TargetPosition.y * 5 + 2);
                }
            }
        }
        
        private void SetDamageCellColor(Color color, int x, int y)
        {
            if (x < 0 ||
                y < 0 ||
                x > 5 * _levelModel.LevelDescription.Size.x - 1 ||
                y > 5 * _levelModel.LevelDescription.Size.y - 1)
            {
                return;
            }
            
            _damageCells[x, y].material.color = color;
        }
    }
}