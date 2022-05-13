using UnityEngine;

namespace MyCompany.Library.Pool
{
    public class GameObjectPool : Pool<GameObjectPoolableHandler>
    {
        private readonly GameObject _prefab;
        private readonly Transform _pooledObjectsParent;
        
        public GameObjectPool(GameObject prefab, Transform pooledObjectsParent, int initialSize)
        {
            _prefab = prefab;
            _pooledObjectsParent = pooledObjectsParent;
            
            SetCreateMethod(CreateElement);
            Initialize(initialSize);
        }

        private GameObjectPoolableHandler CreateElement()
        {
            var gameObject = Object.Instantiate(_prefab, _pooledObjectsParent);
            var handler = new GameObjectPoolableHandler(gameObject);
            handler.GameObject.SetActive(false);
            return handler;
        }
    }
}