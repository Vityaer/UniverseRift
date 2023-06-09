using MainScripts;
using UnityEngine;

namespace UIController.MessagePanels
{
    public class HelpLabelMessage : MonoBehaviour
    {
        public void ShowMessage()
        {
            MessageController.Instance.AddMessage(message);
        }
        [SerializeField] private string message;
    }
}