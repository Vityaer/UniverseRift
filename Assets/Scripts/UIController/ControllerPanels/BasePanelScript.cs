using UnityEngine;

namespace UIController.ControllerPanels
{
    public class BasePanelScript : MonoBehaviour
    {
        public GameObject Panel;

        public virtual void Open()
        {
            Panel.SetActive(true);
            OnOpen();
        }

        public virtual void Close()
        {
            OnClose();
            Panel.SetActive(false);
        }

        protected virtual void OnOpen() { }
        protected virtual void OnClose() { }
    }
}