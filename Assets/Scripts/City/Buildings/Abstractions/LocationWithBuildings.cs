using Fight;
using UIController;
using UnityEngine;

namespace City.Buildings.Abstractions
{
    public class LocationWithBuildings : MonoBehaviour
    {
        public GameObject Location;
        public string TypeBackground;

        public virtual void Open()
        {
            Debug.Log("location open");
            OnOpenLocation();
            Show();
        }

        public virtual void Close()
        {
            LocationController.Instance.Close();
            Hide();
        }

        public void Show()
        {
            LocationController.Instance.OpenLocation(TypeBackground);
            Location.SetActive(true);
        }

        public void Hide()
        {
            Location.SetActive(false);
        }

        protected virtual void OnOpenLocation() { }
    }
}