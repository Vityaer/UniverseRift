using City.Buildings.General;
using City.TrainCamp;
using Common;
using Common.Resourses;
using Models.City.Mines;
using System.Collections.Generic;
using UnityEngine;

namespace City.Buildings.Mines
{
    public class MinesController : Building
    {
        public List<PlaceForMine> minePlaces = new List<PlaceForMine>();
        public List<MineController> buildings = new List<MineController>();

        [Header("Panels")]
        public PanelInfoMine panelMineInfo;
        public PanelForCreateMine panelNewMineCreate;

        [Header("Data")]
        [SerializeField] private List<MineData> listDataMines = new List<MineData>();

        private List<MineModel> saveMines = new List<MineModel>();

        private static MinesController instance;
        public static MinesController Instance { get => instance; }

        void Awake() { if (instance == null) { instance = this; } else { Debug.Log("instance twice"); } }

        protected override void OnLoadGame()
        {
            saveMines = GameController.GetPlayerGame.GetMines;
            PlaceForMine place = null;
            MineController mineController = null;
            MineData data = null;
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

        public void CreateNewMine(PlaceForMine place, MineData data)
        {
            data.AddMine();
            MineController mineController = Instantiate(data.prefabMine, place.point.position, Quaternion.identity, place.transform).GetComponent<MineController>();
            place.mineController = mineController;
            MineModel mine = new MineModel(place.ID, data.type);
            mineController.CreateMine(mine);
        }

        public MineData GetDataMineFromType(TypeMine type)
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
}