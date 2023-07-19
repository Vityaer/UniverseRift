using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Helpers
{
    [RequireComponent(typeof(Toggle))]
    public class ToggleSound : UiHelper, IPointerDownHandler, IPointerUpHandler
    {
        [HideInInspector] [SerializeField] private Toggle _toggle;

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_toggle.isOn == false)
                PlaySound();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
        }

        protected override bool CanWork()
        {
            return _toggle.interactable;
        }

        protected override void GetComponents()
        {
            _toggle = GetComponent<Toggle>();
        }
    }
}