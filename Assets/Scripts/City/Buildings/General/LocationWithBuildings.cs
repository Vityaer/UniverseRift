using Fight;
using UIController;
using UnityEngine;

namespace City.Buildings.General
{
    public class LocationWithBuildings : MonoBehaviour
    {
        public GameObject Location;
        public string TypeBackground;
        public Building BuildingCanvas;

        public virtual void Open()
        {
            Debug.Log("location open");
            MenuController.Instance.CloseMainPage();
            BuildingCanvas.Open();
            OnOpenLocation();
            Show();
        }

        public virtual void Close()
        {
            LocationController.Instance.Close();
            BuildingCanvas.Close();
            MenuController.Instance.OpenMainPage();
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