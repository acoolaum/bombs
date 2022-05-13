using MyCompany.Game.Descriptions;
using MyCompany.Game.Model;
using MyCompany.Library.Pool;
using UnityEngine;

namespace MyCompany.Game.Services
{
    public class BombView
    {
        private readonly Transform _root;
        private readonly LevelDescription _levelDescription;

        public BombModel Model { get; }
        public GameObjectPoolableHandler Handler { get; private set; }
        private GameObject _viewGameObject;
        private Transform _viewTransform;
        
        public BombView(BombModel model, LevelDescription levelDescription)
        {
            Model = model;
            _levelDescription = levelDescription;
        }

        public bool IsExploded => Model.IsExploded;

        public void Show(GameObjectPoolableHandler handler)
        {
            Handler = handler;
            _viewGameObject = Handler.GameObject;
            _viewGameObject.transform.position = GetCurrentPosition();
            var trailRenderer = _viewGameObject.GetComponentInChildren<TrailRenderer>(true);
            if (trailRenderer)
            {
                trailRenderer.Clear();
            }
        }

        public void Update()
        {
            if (Model.Changed)
            {
                _viewGameObject.transform.position = GetCurrentPosition();
            }
        }

        private Vector3 GetCurrentPosition()
        {
            if (Model.CurrentTime >= Model.TotalMoveDuration)
            {
                return _levelDescription.GetWorldCoords(Model.TargetPosition);
            }
            else
            {
                var dt = Model.CurrentTime / Model.TotalMoveDuration;
                return Vector3.Lerp(
                    _levelDescription.GetWorldCoords(Model.StartPosition) + Vector3.up,
                    _levelDescription.GetWorldCoords(Model.TargetPosition),
                    dt) + 3f * (1f - 4f * (0.5f - dt) * (0.5f - dt)) * Vector3.up;
            }
        }
    }
}