using System.Collections.Generic;
using UnityEngine;

public class PlaceForMine : MonoBehaviour
{
    public int ID;
    public Transform point;
    public List<TypeMine> types = new List<TypeMine>();
    public MineController mineController = null;

    void Awake()
    {
        point = base.transform;
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
