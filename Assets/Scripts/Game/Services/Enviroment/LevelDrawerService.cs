using System.Collections.Generic;
using MyCompany.Core.Data;
using MyCompany.Game.Descriptions;
using MyCompany.Game.Model;
using MyCompany.Library.Services;
using MyCompany.Library.UnityEvent;
using UnityEngine;

namespace MyCompany.Game.Services
{
    public class LevelDrawerService : ServiceBase
    {
        private LevelModel _levelModel;
        private UnityEventProxy _unityEventProxy;
        
        private GameObject _levelRootGameObject;

        private List<WallAnimationController> _wallAnimationData;

        public override void Initialize()
        {
            _unityEventProxy = CurrentServices.GetService<UnityEventProxy>();
            _unityEventProxy.Updated += HandleUpdated;
            
            _levelModel = CurrentServices.GetService<LevelModel>();
            _levelModel.WallCells.ElementChanged += HandleWallCellChanged;

            _levelRootGameObject = new GameObject("LevelRoot");
            
            var levelDescription = _levelModel.LevelDescription;

            _wallAnimationData = new List<WallAnimationController>(levelDescription.WallDescription.TotalWallsCount + 
                                                2 * (levelDescription.Size.x - 1) +
                                                2 * (levelDescription.Size.y - 1));
            
            DrawFloor(levelDescription);
            DrawWalls();
        }

        private void DrawFloor(LevelDescription levelDescription)
        {
            var rootTransform = _levelRootGameObject.transform;
            var size = levelDescription.Size;
            
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    var prefab = levelDescription.FloorPrefabs[(x + y) % 2];
                    Object.Instantiate(prefab, levelDescription.GetWorldCoords(x, y), Quaternion.identity, rootTransform);
                }
            }
        }

        private void DrawWalls()
        {
            for (int x = 0; x < _levelModel.WallCells.Size.x; x++)
            {
                for (int y = 0; y < _levelModel.WallCells.Size.y; y++)
                {
                    if (_levelModel.WallCells[x, y])
                    {
                        DrawWall(new Vector2Int(x, y));
                    }
                }
            }
        }

        private void HandleUpdated()
        {
            foreach (var wall in _wallAnimationData)
            {
                wall.Animate(Time.deltaTime);
            }
        }

        public override void Dispose()
        {
            _unityEventProxy.Updated -= HandleUpdated;
            _levelModel.WallCells.ElementChanged -= HandleWallCellChanged;
            
            Object.Destroy(_levelRootGameObject);
        }

        private void HandleWallCellChanged(MapData<bool>.MapDataElementChangedArgs cellChangedArgs)
        {
            if (cellChangedArgs.NewValue)
            {
                DrawWall(cellChangedArgs.Position);
            }
        }

        private void DrawWall(Vector2Int position)
        {
            var rootTransform = _levelRootGameObject.transform;
            var levelDescription = _levelModel.LevelDescription;

            _wallAnimationData.Add(new WallAnimationController(
                rootTransform,
                position,
                levelDescription));
        }

        private class WallAnimationController
        {
            private readonly LevelDescription _levelDescription;
            private readonly LevelWallDescription _wallDescription;
            private readonly Transform _rootTransform;
            private readonly Vector2Int _position;
            
            private WallAnimationBehaviour _animationBehaviour;
            private Transform _objectTransform;

            public WallAnimationController(Transform rootTransform, Vector2Int position, LevelDescription levelDescription)
            {
                _rootTransform = rootTransform;
                _position = position;
                _levelDescription = levelDescription;
                _wallDescription = levelDescription.WallDescription;
                _animationBehaviour = new DelayWallAnimation(this);
            }

            public void Animate(float deltaTime)
            {
                if (_animationBehaviour == null)
                {
                    return;
                }

                _animationBehaviour = _animationBehaviour.Animate(deltaTime);
            }
            
            private abstract class WallAnimationBehaviour
            {
                protected readonly WallAnimationController Controller;
                protected WallAnimationBehaviour(WallAnimationController controller)
                {
                    Controller = controller;
                }

                public abstract WallAnimationBehaviour Animate(float deltaTime);
            }
            
            private class DelayWallAnimation: WallAnimationBehaviour
            {
                private float _delay;
                public DelayWallAnimation(WallAnimationController controller) : base(controller)
                {
                    _delay = Random.Range(0f, Controller._wallDescription.MaxShowDelay);
                }

                public override WallAnimationBehaviour Animate(float deltaTime)
                {
                    _delay -= deltaTime;
                    if (_delay > 0f)
                    {
                        return this;
                    }

                    return new CreateAndMoveWallAnimation(Controller);
                }
            }
            
            private class CreateAndMoveWallAnimation: WallAnimationBehaviour
            {
                private readonly Vector3 _startPosition;
                private readonly Vector3 _targetPosition;
                private readonly float _duration;
                
                private float _currentTime;

                public CreateAndMoveWallAnimation(WallAnimationController controller) : base(controller)
                {
                    _targetPosition = Controller._levelDescription.GetWorldCoords(Controller._position);
                    _startPosition = _targetPosition;
                    _startPosition.y = -1;
                    var gameObject = Object.Instantiate(Controller._wallDescription.WallPrefab, _startPosition, Quaternion.identity,
                        controller._rootTransform);
                    gameObject.name = Controller._position.ToString();
                    Controller._objectTransform = gameObject.transform; 
                    
                    _duration = (_targetPosition - _startPosition).magnitude / Controller._wallDescription.MoveSpeed;
                }

                public override WallAnimationBehaviour Animate(float deltaTime)
                {
                    _currentTime += deltaTime;
                    if (_currentTime < _duration)
                    {
                        Controller._objectTransform.position =
                            Vector3.Lerp(_startPosition, _targetPosition, _currentTime / _duration);
                        return this;
                    }
                    else
                    {
                        Controller._objectTransform.position =_targetPosition;
                        return null;
                    }
                }
            }
        }
    }
}