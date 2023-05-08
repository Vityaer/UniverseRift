using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MineCard : MonoBehaviour
{
    public GameObject panel;
    public Image imageMine, outLight;
    [SerializeField] private TextMeshProUGUI textCountRequirement;
    private DataAboutMines data;
    private static MineCard selectMineCard = null;

    public bool GetCanCreateFromCount { get => (data.currentCount < data.maxCount); }

    public void SetData(TypeMine type)
    {
        data = MinesController.Instance.GetDataMineFromType(type);
        imageMine.sprite = data.image;
        textCountRequirement.text = FunctionHelp.AmountFromRequireCount(data.currentCount, data.maxCount);
        panel.SetActive(true);
    }

    public void Select()
    {
        if (data.currentCount < data.maxCount)
        {
            MinesController.Instance.panelNewMineCreate.UpdateUI(data);
            selectMineCard?.Diselect();
            selectMineCard = this;
            outLight.enabled = true;
        }
    }

    public void Diselect()
    {
        outLight.enabled = false;
    }

    public static void DiselectAfterCreate()
    {
        selectMineCard?.Diselect();
    }
    
    public void Hide()
    {
        panel.SetActive(false);
    }
}
