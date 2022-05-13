using MyCompany.Game.Descriptions;
using MyCompany.Game.Model;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MyCompany.Game.Services
{
    public class CreatureView
    {
        public CreatureModel Model { get; }
        private readonly Transform _root;
        private readonly LevelDescription _levelDescription;
        private readonly CreatureModelProperty _speed;
        
        private GameObject _viewGameObject;
        private Vector3 _baseCoords;
        private float _time;
        private Transform _transform;
        private Vector3 _direction = Vector3.up;
        private Vector3 _offset = Vector3.zero;

        public CreatureView(CreatureModel model, Transform creatureRootTransform, LevelDescription levelDescription)
        {
            Model = model;
            _speed = Model[EffectTargetProperty.Speed];
            _levelDescription = levelDescription;
            _root = creatureRootTransform;
        }

        public void Show()
        {
            if (_viewGameObject == null)
            {
                _viewGameObject = Object.Instantiate(Model.Description.Prefab,
                    _levelDescription.GetWorldCoords(Model.Position), Quaternion.identity, _root);
                _transform = _viewGameObject.transform;
            }
            else
            {
                _transform.position = _levelDescription.GetWorldCoords(Model.Position);
            }

            _baseCoords = _transform.position;
            
            _viewGameObject.SetActive(true);
        }

        public void Dispose()
        {
            Object.Destroy(_viewGameObject);
        }

        public Vector3 GetTooltipPosition()
        {
            return _transform.position + Vector3.up;
        }

        public void Update()
        {
            _offset += 0.3f * _speed.Value / Model.Description.MaxSpeed * Time.deltaTime * _direction;

            if (_offset.y > 0.3f)
            {
                _direction = Vector3.down;
            }
            else if (_offset.y < 0f)
            {
                _offset.y = 0f;
                _direction = Vector3.up;
                ;
            }

            var pos =  _baseCoords + _offset;
            _transform.position = pos;
        }
    }
}