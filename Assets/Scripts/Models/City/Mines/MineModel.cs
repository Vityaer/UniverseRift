using System;
using UnityEngine;

namespace Models.City.Mines
{
    [Serializable]
    public class MineModel : MineBuildModel
    {
        public ResourceModel store;
        [SerializeField] private string previousDateTime;

        public DateTime PreviousDateTime { get => FunctionHelp.StringToDateTime(previousDateTime); set => previousDateTime = value.ToString(); }

        public MineModel() { }

        public MineModel(MineController mineController) : base(mineController)
        {
            ChangeInfo(mineController);
        }

        public MineModel(string ID, TypeMine typeMine) : base(ID, typeMine)
        {
            this.Id = ID;
            this.level = 1;
            store = new ResourceModel(MinesController.GetTypeResourceFromTypeMine(typeMine));
            this.typeMine = typeMine;
            previousDateTime = DateTime.Now.ToString();
        }

        public void ChangeInfo(MineController mineController)
        {
            this.Id = mineController.ID;
            this.level = mineController.GetMine.level;
            this.typeMine = mineController.GetMine.type;
            store = new ResourceModel(mineController.GetMine.GetStore);
            previousDateTime = mineController.GetMine.previousDateTime.ToString();
        }

    }
}
