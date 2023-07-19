using UI.Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Helpers
{
    [RequireComponent(typeof(Button))]
    public class BaseButtonHelper : UiHelper
    {
        [SerializeField] protected Button ButtonComponent;

        protected virtual void Start()
        {
            ButtonComponent.onClick.AddListener(PlaySound);
        }

        protected override void GetComponents()
        {
            ButtonComponent = GetComponent<Button>();
        }

        protected override bool CanWork()
        {
            return ButtonComponent.interactable;
        }
    }
}