using UnityEngine;
using Object = UnityEngine.Object;

namespace MyCompany.Library.Pool
{
    public class GameObjectPoolableHandler: PoolableObjectBase<GameObjectPoolableHandler>
    {
        public GameObject GameObject { get; }

        public GameObjectPoolableHandler(GameObject gameObject)
        {
            GameObject = gameObject;
        }

        public override void Dispose()
        {
            Object.Destroy(GameObject);
        }

        protected override void OnGet()
        {
            GameObject.SetActive(true);
        }

        protected override void OnReturn()
        {
            GameObject.SetActive(false);
        }
    }
}