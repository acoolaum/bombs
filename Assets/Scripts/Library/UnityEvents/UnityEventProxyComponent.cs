using System;
using UnityEngine;

namespace MyCompany.Library.UnityEvent
{
    public class UnityEventProxyComponent : MonoBehaviour
    {
        public event Action Updated;
        public event Action FixedUpdated;
        
        void OnEnable()
        { 
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            Updated?.Invoke();
        }

        private void FixedUpdate()
        {
            FixedUpdated?.Invoke();
        }
    }
}