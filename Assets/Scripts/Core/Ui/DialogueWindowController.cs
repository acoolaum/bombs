using System;
using Object = UnityEngine.Object;

namespace MyCompany.Core.Ui
{
    public class DialogueWindowController: IDisposable
    {
        public event Action ButtonClicked;
        
        private readonly DialogueWindowHierarchy _hierarchy;
        private readonly string _message;

        public DialogueWindowController(DialogueWindowHierarchy hierarchy, string message)
        {
            _hierarchy = hierarchy;
            _message = message;
        }

        public void Show()
        {
            _hierarchy.Message.text = _message;
            _hierarchy.Button.onClick.AddListener(HandleButtonClicked);
            _hierarchy.gameObject.SetActive(true);
        }

        private void HandleButtonClicked()
        {
            ButtonClicked?.Invoke();
        }

        public void Dispose()
        {
            Object.Destroy(_hierarchy.gameObject);
        }
    }
}