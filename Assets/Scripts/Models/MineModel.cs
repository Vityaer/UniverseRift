using System;
using UnityEngine;

namespace Models
{
    [Serializable]
    public class MineModel : MineBuildModel
    {
        public ResourceModel store;
        [SerializeField] private string previousDateTime;
        public DateTime PreviousDateTime { get => FunctionHelp.StringToDateTime(previousDateTime); set => previousDateTime = value.ToString(); }

        public MineModel(MineController mineController) : base(mineController)
        {
            ChangeInfo(mineController);
        }

        public MineModel(string ID, TypeMine typeMine) : base(ID, typeMine)
        {
            this.Id = ID;
            this.level = 1;
            this.store = new ResourceModel(MinesController.GetTypeResourceFromTypeMine(typeMine));
            this.typeMine = typeMine;
            this.previousDateTime = DateTime.Now.ToString();
        }

        public void ChangeInfo(MineController mineController)
        {
            this.Id = mineController.ID;
            this.level = mineController.GetMine.level;
            this.typeMine = mineController.GetMine.type;
            this.store = new ResourceModel(mineController.GetMine.GetStore);
            this.previousDateTime = mineController.GetMine.previousDateTime.ToString();
        }

    }
}
