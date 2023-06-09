using Models.City.Mines;
using System.Collections.Generic;
using UnityEngine;

namespace City.Buildings.Mines
{
    public class PlaceForMine : MonoBehaviour
    {
        public string ID;
        public Transform point;
        public List<TypeMine> types = new List<TypeMine>();
        public MineController mineController = null;

        void Awake()
        {
            point = transform;
        }

        public void OpenPanelForCreateMine()
        {
            if (mineController == null)
            {
                MinesController.Instance.panelNewMineCreate.Open(this);
            }
            else
            {
                MinesController.Instance.panelMineInfo.SetData(mineController);
            }
        }
    }
}