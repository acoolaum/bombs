using MyCompany.Library.Services;
using UnityEngine;

namespace MyCompany.Core.Ui
{
    public class UiController: ServiceBase
    {
        private readonly GameObject _uiPrefab;
        
        private GameObject _uiGameObject;
        private UiHierarchy _hierarchy;

        public UiController(GameObject uiPrefab)
        {
            _uiPrefab = uiPrefab;
        }
        
        public override void Initialize()
        {
            _uiGameObject = Object.Instantiate(_uiPrefab);
            _hierarchy = _uiGameObject.GetComponent<UiHierarchy>();
        }

        public override void Dispose()
        {
            Object.Destroy(_uiGameObject);
        }

        public DialogueWindowController CreateDialogueWindow(GameObject prefab, string message)
        {
            var o = Object.Instantiate(prefab, _hierarchy.FrontPanel);
            var hierarchy = o.GetComponent<DialogueWindowHierarchy>();
            var controller = new DialogueWindowController(hierarchy, message);
            return controller;
        }

        public GameObject CreatePanel(string panelName)
        {
            var o = new GameObject(panelName);
            o.transform.SetParent(_hierarchy.StandardPanel);
            return o;
        }
    }
}