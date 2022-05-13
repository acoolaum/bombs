using System;
using DG.Tweening;
using MyCompany.Library.Services;
using UnityEngine;

namespace MyCompany.Library.UnityEvent
{
    public class UnityEventProxy : ServiceBase
    {
        private class PeriodicalUpdater : IDisposable
        {
            private bool Disposed { get; set; }

            private readonly UnityEventProxy _unityEventProxy;
            private Tween _updateTween;

            public PeriodicalUpdater(UnityEventProxy unityEventProxy, float period, TweenCallback callback)
            {
                _unityEventProxy = unityEventProxy;
                _unityEventProxy.Disposing += HandleDisposing;

                _updateTween = DOVirtual.DelayedCall(period, callback).SetLoops(-1);
            }

            private void HandleDisposing()
            {
                if (Disposed)
                {
                    return;
                }

                Disposed = true;
                _updateTween?.Kill();
                _updateTween = null;
            }

            public void Dispose()
            {
                HandleDisposing();
                _unityEventProxy.Disposing -= Dispose;
            }
        }

        public event Action Updated;
        public event Action FixedUpdated;
        public event Action Disposing;

        private GameObject _proxyGameObject;
        private UnityEventProxyComponent _proxy;

        public override void Initialize()
        {
            _proxyGameObject = new GameObject("UnityEventProxy_" + Layer, typeof(UnityEventProxyComponent));
            _proxy = _proxyGameObject.GetComponent<UnityEventProxyComponent>();
            _proxy.Updated += HandleUpdate;
            _proxy.FixedUpdated += HandleFixedUpdated;
        }

        public IDisposable CreatePeriodicalUpdater(float period, TweenCallback callback)
        {
            var updater = new PeriodicalUpdater(this, period, callback);
            return updater;
        }

        private void HandleUpdate()
        {
            Updated?.Invoke();
        }        
        
        private void HandleFixedUpdated()
        {
            FixedUpdated?.Invoke();
        }

        public override void Dispose()
        {
            var action = Disposing;
            action?.Invoke();
            UnityEngine.Object.Destroy(_proxyGameObject);
        }
    }
}