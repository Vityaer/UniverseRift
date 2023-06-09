using UnityEngine;
using UnityEngine.UI;

namespace UIController.Other
{
    public class CustomToggle : MonoBehaviour
    {
        public Image imageToggle;
        [SerializeField] private bool isOn = false;
        public Sprite spriteIsOn, spriteIsOff;

        [ContextMenu("ChangeState")]
        public void ChangeState()
        {
            isOn = !isOn;
            imageToggle.sprite = isOn ? spriteIsOn : spriteIsOff;
        }
    }
}