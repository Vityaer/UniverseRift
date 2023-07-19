using City.Buildings.Mines;
using Common.Resourses;
using System;
using UnityEngine;

namespace Models.City.Mines
{
    [Serializable]
    public class MineModel : MineBuildModel
    {
        public GameResource Store;
        [SerializeField] private string _previousDateTime;

        public DateTime PreviousDateTime { get => FunctionHelp.StringToDateTime(_previousDateTime); set => _previousDateTime = value.ToString(); }

        public MineModel() { }

        public MineModel(MineController mineController) : base(mineController)
        {
            ChangeInfo(mineController);
        }

        public MineModel(string ID, TypeMine typeMine) : base(ID, typeMine)
        {
            this.Id = ID;
            this.level = 1;
            Store = new GameResource(MinesController.GetTypeResourceFromTypeMine(typeMine));
            this.typeMine = typeMine;
            _previousDateTime = DateTime.Now.ToString();
        }

        public void ChangeInfo(MineController mineController)
        {
            this.Id = mineController.ID;
            this.level = mineController.GetMine.level;
            this.typeMine = mineController.GetMine.type;
            //Store = mineController.GetMine.GetStore.Clone();
            _previousDateTime = mineController.GetMine.previousDateTime.ToString();
        }

    }
}
