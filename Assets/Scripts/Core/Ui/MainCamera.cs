using MyCompany.Library.Services;
using UnityEngine;

namespace MyCompany.Core.Ui
{
    public class MainCamera : ServiceBase
    {
        private readonly Camera _camera;

        public MainCamera(Camera camera)
        {
            _camera = camera;
        }

        public Vector2 WorldToScreenPoint(Vector3 position)
        {
            return _camera.WorldToScreenPoint(position);
        }
    }
}