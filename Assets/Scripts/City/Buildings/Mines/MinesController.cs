using Models.City.Mines;
using System.Collections.Generic;
using UnityEngine;

public class MinesController : Building
{
    public List<PlaceForMine> minePlaces = new List<PlaceForMine>();
    public List<MineController> buildings = new List<MineController>();

    [Header("Panels")]
    public PanelInfoMine panelMineInfo;
    public PanelForCreateMine panelNewMineCreate;

    [Header("Data")]
    [SerializeField] private List<DataAboutMines> listDataMines = new List<DataAboutMines>();

    private List<MineModel> saveMines = new List<MineModel>();

    private static MinesController instance;
    public static MinesController Instance { get => instance; }

    void Awake() { if (instance == null) { instance = this; } else { Debug.Log("instance twice"); } }

    protected override void OnLoadGame()
    {
        saveMines = GameController.GetPlayerGame.GetMines;
        PlaceForMine place = null;
        MineController mineController = null;
        DataAboutMines data = null;
        foreach (MineModel mine in saveMines)
        {
            place = minePlaces.Find(x => x.ID == mine.Id);
            if (place != null)
            {
                data = listDataMines.Find(x => x.type == mine.typeMine);
                data.AddMine();
                mineController = Instantiate(data.prefabMine, place.point.position, Quaternion.identity, place.transform).GetComponent<MineController>();
                place.mineController = mineController;
                mineController.LoadMine(mine);
                buildings.Add(mineController);
            }
            else
            {
                Debug.Log(string.Concat("place with ID = ", mine.Id.ToString(), " not found"));
            }
        }
    }

    public void CreateNewMine(PlaceForMine place, DataAboutMines data)
    {
        data.AddMine();
        MineController mineController = Instantiate(data.prefabMine, place.point.position, Quaternion.identity, place.transform).GetComponent<MineController>();
        place.mineController = mineController;
        MineModel mine = new MineModel(place.ID, data.type);
        mineController.CreateMine(mine);
    }

    public DataAboutMines GetDataMineFromType(TypeMine type)
    {
        return listDataMines.Find(x => x.type == type);
    }

    public CostLevelUp GetCostUpdateMine(TypeMine type)
    {
        return listDataMines.Find(x => x.type == type).ResourceOnLevelUP;
    }

    public static TypeMine GetTypeMineFromTypeResource(TypeResource typeResource)
    {
        TypeMine result = TypeMine.Gold;
        switch (typeResource)
        {
            case TypeResource.Gold:
                result = TypeMine.Gold;
                break;
            case TypeResource.Diamond:
                result = TypeMine.Diamond;
                break;
            case TypeResource.RedDust:
                result = TypeMine.RedDust;
                break;
        }
        return result;
    }

    public static TypeResource GetTypeResourceFromTypeMine(TypeMine typeMine)
    {
        TypeResource result = TypeResource.Gold;
        switch (typeMine)
        {
            case TypeMine.Gold:
                result = TypeResource.Gold;
                break;
            case TypeMine.Diamond:
                result = TypeResource.Diamond;
                break;
            case TypeMine.RedDust:
                result = TypeResource.RedDust;
                break;
        }
        return result;
    }
}

[System.Serializable]
public class DataAboutMines
{
    public TypeMine type;
    public int currentCount = 0, maxCount = 2;
    public CostLevelUp ResourceOnLevelProduction, ResourceOnLevelUP;
    public int maxStore = 50;
    public TypeStore typeStore = TypeStore.Percent;
    public ListResource costCreate;
    public GameObject prefabMine;
    public Sprite image { get => prefabMine.GetComponent<SpriteRenderer>().sprite; }
    public void AddMine()
    {
        currentCount += 1;
    }
}