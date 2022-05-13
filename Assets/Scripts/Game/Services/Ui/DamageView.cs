using System;
using DG.Tweening;
using MyCompany.Library.Pool;
using UnityEngine;

namespace MyCompany.Game.Services
{
    public class DamageView : IDisposable
    {
        public event Action<DamageView> Completed; 
        
        private readonly GameObjectPoolableHandler _handler;
        private readonly GameObject _gameObject;
        private readonly DamageViewHierarchy _hierarchy;
        private Sequence _sequence;

        public DamageView(GameObjectPoolableHandler handler)
        {
            _handler = handler;
            _gameObject = _handler.GameObject;
            _hierarchy = _gameObject.GetComponent<DamageViewHierarchy>();
        }

        public void Show(Vector3 screenPosition, string text, float duration)
        {
            _gameObject.SetActive(true);
            _gameObject.transform.position = screenPosition;
            _hierarchy.Text.text = text;
            _hierarchy.Text.color = new Color(1f, 0.5f, 0.5f, 1f);

            _sequence = DOTween.Sequence();
            _sequence.Append(_gameObject.transform.DOBlendableLocalMoveBy(150 * Vector3.up, duration));
            _sequence.Join(DOVirtual.Float(1f, 0f, 2f, alpha =>
            {
                _hierarchy.Text.color = new Color(1f, 0.5f, 0.5f, alpha);
            }).SetEase(Ease.InCubic));
            _sequence.AppendCallback(() =>
            {
                _handler.Return();
                Completed?.Invoke(this);
            });
        }

        public void Dispose()
        {
            _sequence?.Kill();
        }
    }
}