using Common;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace City.Buildings.General
{
    public class Building : SerializedMonoBehaviour
    {
        [SerializeField] protected GameObject building;
        [SerializeField] protected Button buttonOpenBuilding, buttonCloseBuilding;
        [Header("Main settings")]
        [SerializeField] private int levelForAvailableBuilding = 0;

        protected virtual void Start()
        {
            OnStart();
            GameController.Instance.RegisterOnLoadGame(OnLoadGame);
            building.SetActive(false);
            buttonOpenBuilding?.onClick.RemoveAllListeners();
            buttonCloseBuilding?.onClick.RemoveAllListeners();
            buttonOpenBuilding?.onClick.AddListener(() => Open());
            buttonCloseBuilding?.onClick.AddListener(() => Close());
        }

        public virtual void Open()
        {
            if (AvailableFromLevel())
            {
                CanvasBuildingsUI.Instance.OpenBuilding(building);
                OpenPage();
            }
        }

        protected bool AvailableFromLevel()
        {
            bool result = GameController.GetPlayerInfo.Level >= levelForAvailableBuilding;
            if (result == false)
                MessageController.Instance.ShowErrorMessage($"Откроется на {levelForAvailableBuilding} уровне");

            return result;
        }

        public virtual void Close()
        {
            ClosePage();
            CanvasBuildingsUI.Instance.CloseBuilding(building);
        }

        virtual protected void OnStart() { }
        virtual protected void OpenPage() { }
        virtual protected void ClosePage() { }
        virtual protected void OnLoadGame() { }

        protected void SaveGame()
        {
            GameController.Instance.SaveGame();
        }
    }
}