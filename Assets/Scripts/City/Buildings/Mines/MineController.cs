using Models.City.Mines;
using UnityEngine;
using UnityEngine.UI;

public class MineController : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private string id;
    [SerializeField] private Mine mine;
    [Header("UI")]
    [SerializeField] private Image image;
    public Animator anim;

    public Mine GetMine { get => mine; }
    public string ID { get => id; }

    public void LoadMine(MineModel mineSave)
    {
        mine.SetData(mineSave);
    }

    public void CreateMine(MineModel mineSave)
    {
        // anim?.Play("Create");
        mine.SetData(mineSave);
        GameController.GetPlayerGame.SaveMine(this);
    }

    public void UpdateLevel()
    {
        mine.LevelUP();
        GameController.GetPlayerGame.SaveMine(this);
    }

    public void GetReward()
    {
        mine.GetResources();
        GameController.GetPlayerGame.SaveMine(this);
    }

    public void OpenPanelInfo()
    {
        MinesController.Instance.panelMineInfo.SetData(this);
    }
}
public enum TypeMine
{
    Diamond = 0,
    Gold = 1,
    RedDust = 2,
    Energy = 100,
    Attack = 101,
    HP = 102,
    Main = 1000
}
public enum TypeStore
{
    Percent = 0,
    Num = 1
}